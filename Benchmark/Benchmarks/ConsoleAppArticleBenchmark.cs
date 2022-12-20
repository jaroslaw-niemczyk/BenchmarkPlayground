using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[Config(typeof(Config))]
[MemoryDiagnoser]
public class ConsoleAppArticleBenchmark
{
    private string _sampleString = new('a', 1000);
    private int _a = 1;
    private int _b = 10;

    private int rows = 100;

    [Benchmark]
    public int SmallStruct()
    {
        int sum = 0;
        for (int i = 0; i < rows; i++)
        {
            SmallStruct test = new(_a, _b);
            sum += test.A + test.B;
        }
        return sum;
    }

    [Benchmark]
    public int MediumStruct()
    {
        int sum = 0;
        for (int i = 0; i < rows; i++)
        {
            MediumStruct test = new(_a, _b, _sampleString, _sampleString);
            sum += test.A + test.B;
        }
        return sum;
    }

    [Benchmark]
    public int SmallClass()
    {
        int sum = 0;
        for (int i = 0; i < rows; i++)
        {
            SmallClass test = new(_a, _b);
            sum += test.A + test.B;
        }
        return sum;
    }

    [Benchmark]
    public int MediumClass()
    {
        int sum = 0;
        for (int i = 0; i < rows; i++)
        {
            MediumClass test = new(_a, _b, _sampleString, _sampleString);
            sum =+ test.A + test.B;
        }
        return sum;
    }
}

public struct SmallStruct
{
    public int A { get; }
    public int B { get; }

    public SmallStruct(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}

public struct MediumStruct
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }

    public MediumStruct(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}

public class SmallClass
{
    public int A { get; }
    public int B { get; }

    public SmallClass(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}

public class MediumClass
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }

    public MediumClass(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}