using Benchmark.Benchmarks;
using BenchmarkDotNet.Running;
using WebAppMvc.Benchmarks;

//BenchmarkRunner.Run<ConsoleAppArticleBenchmark>();
//BenchmarkRunner.Run<ConsoleAppMvcMimicBenchmark>();
// BenchmarkRunner.Run<BenchmarkWebAppMvc>();
// BenchmarkRunner.Run<JsonConverterBenchmark>();


var jsonConverterBenchmark = new JsonConverterBenchmark();

jsonConverterBenchmark.UsedTypedEngineDeserialization();
