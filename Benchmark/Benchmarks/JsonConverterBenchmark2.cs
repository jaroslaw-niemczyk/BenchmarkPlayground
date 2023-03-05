﻿using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

// [Config(typeof(Config))]
[MemoryDiagnoser]
public class JsonConverterBenchmark2
{
    private int rows = 100;
    
    [Benchmark]
    public int UsedReflection()
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
                Values = new Dictionary<string, object>
                {
                    {"IntegerP", 1 },
                    {"StringP", "test" },
                    {"DtoCh", new { IntegerProperty = 2, StringProperty = "testChild"}}
                }
            });
        
        for (int i = 0; i < rows; i++)
        {
            var externalValues = JsonSerializer.Deserialize<object[]>(jsonStringEngine);
            var formValues = JsonSerializer.Deserialize<FormValues>(jsonStringForm);

            var dto = new Dto();

            foreach (JsonElement jsonElementEngine in externalValues)
            {
                var externalName = jsonElementEngine.GetProperty("ExternalName").GetString();
                var formName = jsonElementEngine.GetProperty("FormName").GetString();
                
                if (externalName == "IntegerProperty")
                {
                    var jsonValue = (JsonElement) formValues.Values.FirstOrDefault(x => x.Key == formName).Value;
                    typeof(Dto).GetProperty(externalName).SetValue(dto, jsonValue.GetInt32());
                }
                
                if (externalName == "StringProperty")
                {
                    var jsonValue = (JsonElement) formValues.Values.FirstOrDefault(x => x.Key == formName).Value;
                    typeof(Dto).GetProperty(externalName).SetValue(dto, jsonValue.GetString());
                }
                
                if (externalName == "DtoChild")
                {
                    var jsonValue = ((JsonElement) formValues.Values.FirstOrDefault(x => x.Key == formName).Value).GetRawText();
                    var jsonObjectValue = JsonSerializer.Deserialize<DtoChild>(jsonValue);
                    typeof(Dto).GetProperty(externalName).SetValue(dto, jsonObjectValue);
                }

            }

            iteration++;
        }

        return iteration;
    }
    
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
                Values = new Dictionary<string, object>
                {
                    {"IntegerP", 1 },
                    {"StringP", "test" },
                    {"DtoCh", new { IntegerProperty = 2, StringProperty = "testChild"}}
                }
            });
        
        for (int i = 0; i < rows; i++)
        {
            var engineValues = JsonSerializer.Deserialize<EngineResponse[]>(jsonStringEngine);
            
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.Converters.Add(new FieldJsonConverter());
            var formValues = JsonSerializer.Deserialize<Field[]>(jsonStringForm, options);

            var dto = new Dto();

            foreach (EngineResponse jsonElementEngine in engineValues)
            {
                var externalName = jsonElementEngine.ExternalName;
                var formName = jsonElementEngine.FormName;
                
                if (externalName == "IntegerProperty")
                {
                    var jsonValue = formValues.FirstOrDefault(x => x.Name == formName).Value;
                    typeof(Dto).GetProperty(externalName).SetValue(dto, (int) jsonValue);
                }
                
                if (externalName == "StringProperty")
                {
                    var jsonValue = formValues.FirstOrDefault(x => x.Name == formName).Value;
                    typeof(Dto).GetProperty(externalName).SetValue(dto, (string) jsonValue);
                }
                
                if (externalName == "DtoChild")
                {
                    var jsonValue = formValues.FirstOrDefault(x => x.Name == formName).Value;
                    typeof(Dto).GetProperty(externalName).SetValue(dto, (DtoChild) jsonValue);
                }
            }

            iteration++;
        }

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
        
        reader.Read(); //PropertyName
        reader.Read(); //StartObject
        
        while (reader.Read()) //PropertyName or StartObject
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var key = reader.GetString();
                reader.Read();
                var field = new Field { Name = key! };

                field.Value = reader.TokenType switch
                {
                    JsonTokenType.Number => reader.GetInt32(),
                    JsonTokenType.String => reader.GetString()!,
                    JsonTokenType.StartObject => (key switch
                    {
                        "DtoCh" => JsonSerializer.Deserialize<DtoChild>(ref reader),
                        _ => field.Value
                    })!,
                    _ => field.Value
                };
                fields.Add(field);
            }
        }

        return fields.ToArray();
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
    //todo add property 
    public IDictionary<string, object> Values { get; set; }
}

public class Field
{
    public string Name { get; set; }
    public object Value { get; set; }
}
