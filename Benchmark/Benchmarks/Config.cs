using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmark.Benchmarks;

class Config : ManualConfig
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