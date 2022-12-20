Simple setup for .NETBenchmark tests

### How to use it?

Program.cs in Benchmark contains 3 benchmarks 

- ConsoleAppArticleBenchmark - a code from [Benjamin repo](https://github.com/BenjaminAbt/SustainableCode/tree/main/csharp/struct-vs-class)
- ConsoleAppMvcMimicBenchmark - a basic code web app structure (controller, service, repo)
- BenchmarkApiMemoryNoReference - a simple WebAppMvc app

To run each tests please use this command for Benchmark project:
```
dotnet run -c Release
```

### Config

Each test are using configuration from a Config.cs file where are defined different GC modes:

-	Non-Concurrent Workstation
-	Background Workstation
-	Non-Concurrent Workstation with forced full GC
-	Background Workstation with forced full GC
-	Server Non-Concurrent Server
-	Background Server
-	Server Non-Concurrent Server with forced full GC
-	Background Server with forced full GC