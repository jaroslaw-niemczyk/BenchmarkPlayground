using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

// [Config(typeof(Config))]
[MemoryDiagnoser]
public class JsonConverterBenchmark
{
    private int rows = 100;
    
    // [Benchmark]
    // public int UsedReflection()
    // {
    //     int iteration = 0;
    //
    //     var jsonStringEngine = JsonSerializer.Serialize(
    //         new object[]
    //             {
    //                 new { FormName ="IntegerP", ExternalName = "IntegerProperty"},
    //                 new { FormName = "StringP", ExternalName = "StringProperty"},
    //                 new { FormName = "DtoCh",  ExternalName = "DtoChild"}
    //             });
    //
    //     var jsonStringForm = JsonSerializer.Serialize(
    //         new {
    //             Values = new Dictionary<string, object>
    //             {
    //                 {"IntegerP", 1 },
    //                 {"StringP", "test" },
    //                 {"DtoCh", new { IntegerProperty = 2, StringProperty = "testChild"}}
    //             }
    //         });
    //     
    //     for (int i = 0; i < rows; i++)
    //     {
    //         var externalValues = JsonSerializer.Deserialize<object[]>(jsonStringEngine);
    //         var formValues = JsonSerializer.Deserialize<FormValues>(jsonStringForm);
    //
    //         var dto = new Dto();
    //
    //         foreach (JsonElement jsonElementEngine in externalValues)
    //         {
    //             var externalName = jsonElementEngine.GetProperty("ExternalName").GetString();
    //             var formName = jsonElementEngine.GetProperty("FormName").GetString();
    //             
    //             if (externalName == "IntegerProperty")
    //             {
    //                 var jsonValue = (JsonElement) formValues.Values.FirstOrDefault(x => x.Key == formName).Value;
    //                 typeof(Dto).GetProperty(externalName).SetValue(dto, jsonValue.GetInt32());
    //             }
    //             
    //             if (externalName == "StringProperty")
    //             {
    //                 var jsonValue = (JsonElement) formValues.Values.FirstOrDefault(x => x.Key == formName).Value;
    //                 typeof(Dto).GetProperty(externalName).SetValue(dto, jsonValue.GetString());
    //             }
    //             
    //             if (externalName == "DtoChild")
    //             {
    //                 var jsonValue = ((JsonElement) formValues.Values.FirstOrDefault(x => x.Key == formName).Value).GetRawText();
    //                 var jsonObjectValue = JsonSerializer.Deserialize<DtoChild>(jsonValue);
    //                 typeof(Dto).GetProperty(externalName).SetValue(dto, jsonObjectValue);
    //             }
    //
    //         }
    //
    //         iteration++;
    //     }
    //
    //     return iteration;
    // }
    
    [Benchmark]
    public int UsedTypedEngineDeserialization()
    {
        int iteration = 0;

        var jsonStringEngine = JsonSerializer.Serialize(
            new object[]
                {
                    new { FormName ="IntegerP", ExternalName = "IntegerProperty"},
                    new { FormName = "StringP", ExternalName = "StringProperty"},
                    new { FormName = "DtoCh",  ExternalName = "DtoChild"}
                });

        var jsonStringForm = JsonSerializer.Serialize(
            new {
                Field = "testField",
                Values = new Dictionary<string, object>
                {
                    {"IntegerP", 1 },
                    {"StringP", "test" },
                    {"DtoCh", new { IntegerProperty = 2, StringProperty = "testChild"}},
                    {"DtoCh3", new { IntegerProperty = 3, StringProperty = "testChild3"}}
                }
            });
        
        // for (int i = 0; i < rows; i++)
        // {
            var engineValues = JsonSerializer.Deserialize<EngineResponse[]>(jsonStringEngine);
            
            var fileds = JsonSerializer.Deserialize<FormValues>(jsonStringForm);

            var dto = new Dto();
            
            foreach (EngineResponse jsonElementEngine in engineValues)
            {
                var externalName = jsonElementEngine.ExternalName;
                var formName = jsonElementEngine.FormName;
                
                if (externalName == "IntegerProperty")
                {
                    var jsonValue = fileds.Values.FirstOrDefault(x => x.Name == formName).Value;
                    typeof(Dto).GetProperty(externalName).SetValue(dto, (int) jsonValue);
                }
                
                if (externalName == "StringProperty")
                {
                    var jsonValue = fileds.Values.FirstOrDefault(x => x.Name == formName).Value;
                    typeof(Dto).GetProperty(externalName).SetValue(dto, (string) jsonValue);
                }
                
                if (externalName == "DtoChild")
                {
                    var jsonValue = fileds.Values.FirstOrDefault(x => x.Name == formName).Value;
                    typeof(Dto).GetProperty(externalName, BindingFlags.IgnoreCase).SetValue(dto, (DtoChild) jsonValue);
                }
            }

        //     iteration++;
        // }

        return iteration;
    }
}

public class FieldJsonConverter : JsonConverter<Field[]>
{
    public override Field[] Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        
        var fields = new List<Field>();
        var startDepth = reader.CurrentDepth;
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == startDepth)
            {
                return fields.ToArray();
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var key = reader.GetString();
                reader.Read();
                var field = new Field { Name = key! };

                switch (reader.TokenType)
                {
                    case JsonTokenType.Number:
                    {
                        field.Value = reader.GetInt32();
                        break;
                    }
                    case JsonTokenType.String:
                    {
                        field.Value = reader.GetString()!;
                        break;
                    }
                    case JsonTokenType.StartObject:
                    {
                        switch (key)
                        {
                            case "DtoCh":
                            {
                                field.Value = JsonSerializer.Deserialize<DtoChild>(ref reader)!;
                                break;
                            }
                            case "DtoCh3":
                            {
                                field.Value = JsonSerializer.Deserialize<DtoChild>(ref reader)!;
                                break;
                            }
                        }
                        break;
                    }
                }
                fields.Add(field);
            }
        }
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Field[] value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

public class EngineResponse
{
    public string FormName { get; set; }
    public string ExternalName { get; set; }
}

public class Dto
{
    public int IntegerProperty { get; set; }
    public string StringProperty { get; set; }
    public DtoChild DtoChild { get; set; }
}

public class DtoChild
{
    public int IntegerProperty { get; set; }
    public string StringProperty { get; set; }
}

public class FormValues
{
    public string Field { get; set; }

    [JsonConverter(typeof(FieldJsonConverter))]
    public Field[] Values { get; set; }
}

public class Field
{
    public string Name { get; set; }
    public object Value { get; set; }
}
