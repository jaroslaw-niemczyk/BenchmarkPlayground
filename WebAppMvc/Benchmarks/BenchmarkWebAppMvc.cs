using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAppMvc.Dto;

namespace WebAppMvc.Benchmarks;

[Config(typeof(Config))]
[MemoryDiagnoser]
[RankColumn]
public class BenchmarkWebAppMvc
{
    private static HttpClient _httpClient = null!;

    [Params(2000)]
    //[Params(1)]
    public int N;
    
    private class Config : ManualConfig
    {
        public Config()
        {
            AddJob(Job.MediumRun.WithGcServer(true).WithGcConcurrent(true).WithGcForce(true).WithId("BackgroundServerForce"));
            AddJob(Job.MediumRun.WithGcServer(true).WithGcConcurrent(true).WithGcForce(false).WithId("BackgroundServer"));
            
            AddJob(Job.MediumRun.WithGcServer(true).WithGcConcurrent(false).WithGcForce(true).WithId("Non-ConcurrentServerForce"));
            AddJob(Job.MediumRun.WithGcServer(true).WithGcConcurrent(false).WithGcForce(false).WithId("Non-ConcurrentServer"));

            AddJob(Job.MediumRun.WithGcServer(false).WithGcConcurrent(true).WithGcForce(true).WithId("BackgroundWorkstationForce"));
            AddJob(Job.MediumRun.WithGcServer(false).WithGcConcurrent(true).WithGcForce(false).WithId("BackgroundWorkstation"));
            
            AddJob(Job.MediumRun.WithGcServer(false).WithGcConcurrent(false).WithGcForce(true).WithId("Non-ConcurrentWorkstationForce"));
            AddJob(Job.MediumRun.WithGcServer(false).WithGcConcurrent(false).WithGcForce(false).WithId("Non-ConcurrentWorkstation"));
        }
    }
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(configuration =>
            {
                configuration.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                });
            });
        _httpClient = factory.CreateClient();
    }

    [Benchmark]
    public async Task<int> GetClassResponseDto()
    {
        var request = new ClassRequestDto
        {
            Id = 1,
            Name = "Name"
        };
        var jitDeadCodeElimination = 0;
        
        for(int i = 0; i < N; i++)
        {
            jitDeadCodeElimination++;
            await _httpClient.PostAsJsonAsync("/DtoBenchmark/GetClassResponseDto", request);
            // var response = await _httpClient.PostAsJsonAsync("/DtoBenchmark/GetClassResponseDto", request);
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        return jitDeadCodeElimination;
    }

    [Benchmark]
    public async Task<int> GetStructResponseDto()
    {
        var request = new StructRequestDto
        {
            Id = 1,
            Name = "Name"
        };
        var jitDeadCodeElimination = 0;

        for (int i = 0; i < N; i++)
        {
            jitDeadCodeElimination++;
            await _httpClient.PostAsJsonAsync("/DtoBenchmark/GetStructResponseDto", request);
            // var response = await _httpClient.PostAsJsonAsync("/DtoBenchmark/GetStructResponseDto", request);
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        return jitDeadCodeElimination;
    }
    
    [Benchmark]
    public async Task<int> GetRecordResponseDto()
    {
        var request = new RecordRequestDto(1, "Name");
        var jitDeadCodeElimination = 0;

        for (int i = 0; i < N; i++)
        {
            jitDeadCodeElimination++;
            await _httpClient.PostAsJsonAsync("/DtoBenchmark/GetRecordResponseDto", request);
            // var response = await _httpClient.PostAsJsonAsync("/DtoBenchmark/GetRecordResponseDto", request);
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        return jitDeadCodeElimination;
    }
}