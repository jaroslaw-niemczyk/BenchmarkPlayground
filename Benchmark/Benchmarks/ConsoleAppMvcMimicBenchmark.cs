using BenchmarkDotNet.Attributes;

namespace Benchmark.Benchmarks;

[Config(typeof(Config))]
[MemoryDiagnoser]
[RankColumn]
public class ConsoleAppMvcMimicBenchmark
{
    private string _sampleString = new('a', 1000);
    private int _a = 1;
    private int _b = 10;

    private int requests = 100;

    [Benchmark]
    public int StructSmall()
    {
        int sum = 0;
        for (int i = 0; i < requests; i++)
        {
            var controller = new Controller(new Service(new Repository()));
            sum += controller.StructSmall(new StructSmallRequest(_a, _b)).A;
        }

        return sum;
    }

    [Benchmark]
    public int StructMedium()
    {
        int sum = 0;
        for (int i = 0; i < requests; i++)
        {
            var controller = new Controller(new Service(new Repository()));
            sum += controller.StructMedium(new StructMediumRequest(_a, _b, _sampleString, _sampleString)).A;
        }

        return sum;
    }

    [Benchmark]
    public int ClassSmall()
    {
        int sum = 0;
        for (int i = 0; i < requests; i++)
        {
            var controller = new Controller(new Service(new Repository()));
            sum += controller.ClassSmall(new ClassSmallRequest(_a, _b)).A;
        }

        return sum;
    }

    [Benchmark]
    public int ClassMedium()
    {
        int sum = 0;
        for (int i = 0; i < requests; i++)
        {
            var controller = new Controller(new Service(new Repository()));
            sum += controller.ClassMedium(new ClassMediumRequest(_a, _b, _sampleString, _sampleString)).A;
        }

        return sum;
    }
}

#region Basic Get MVC classes

public class Controller
{
    private readonly Service _service;

    public Controller(Service service)
    {
        _service = service;
    }
    
    public StructSmallResponse StructSmall(StructSmallRequest structSmallRequest)
    {
        return _service.HandleStructSmall(new StructSmallServiceDto(structSmallRequest.A, structSmallRequest.B));
    }
    
    public StructMediumResponse StructMedium(StructMediumRequest structMediumRequest)
    {
        return _service.HandleStructMedium(new StructMediumServiceDto(structMediumRequest.A, structMediumRequest.B, structMediumRequest.X, structMediumRequest.Y));
    }
    
    public ClassSmallResponse ClassSmall(ClassSmallRequest classSmallRequest)
    {
        return _service.HandleClassSmall(new ClassSmallServiceDto(classSmallRequest.A, classSmallRequest.B));
    }
    
    public ClassMediumResponse ClassMedium(ClassMediumRequest classMediumRequest)
    {
        return _service.HandleClassMedium(new ClassMediumServiceDto(classMediumRequest.A, classMediumRequest.B, classMediumRequest.X, classMediumRequest.Y));
    }
}

public class Service
{
    private readonly Repository _repository;

    public Service(Repository repository)
    {
        _repository = repository;
    }

    public StructSmallResponse HandleStructSmall(StructSmallServiceDto structSmallServiceDto)
    {
        return _repository.GetStructSmall(new StructSmallRepositoryDto(structSmallServiceDto.A, structSmallServiceDto.B));
    }
    
    public StructMediumResponse HandleStructMedium(StructMediumServiceDto structMediumServiceDto)
    {
        return _repository.GetStructMedium(new StructMediumRepositoryDto(structMediumServiceDto.A, structMediumServiceDto.B, structMediumServiceDto.X, structMediumServiceDto.Y));
    }
    
    public ClassSmallResponse HandleClassSmall(ClassSmallServiceDto classSmallServiceDto)
    {
        return _repository.GetClassSmall(new ClassSmallRepositoryDto(classSmallServiceDto.A, classSmallServiceDto.B));
    }
    
    public ClassMediumResponse HandleClassMedium(ClassMediumServiceDto classMediumServiceDto)
    {
        return _repository.GetClassMedium(new ClassMediumRepositoryDto(classMediumServiceDto.A, classMediumServiceDto.B, classMediumServiceDto.X, classMediumServiceDto.Y));
    }
}

public class Repository
{
    public StructSmallResponse GetStructSmall(StructSmallRepositoryDto structSmallRepositoryDto)
    {
        return new StructSmallResponse(
            structSmallRepositoryDto.A + structSmallRepositoryDto.A,
            structSmallRepositoryDto.B + structSmallRepositoryDto.B
        );
    }
    
    public StructMediumResponse GetStructMedium(StructMediumRepositoryDto structMediumRepositoryDto)
    {
        return new StructMediumResponse(
            structMediumRepositoryDto.A + structMediumRepositoryDto.A,
            structMediumRepositoryDto.B + structMediumRepositoryDto.B,
            structMediumRepositoryDto.X,
            structMediumRepositoryDto.Y
        );
    }
    
    public ClassSmallResponse GetClassSmall(ClassSmallRepositoryDto classSmallRepositoryDto)
    {
        return new ClassSmallResponse(
            classSmallRepositoryDto.A + classSmallRepositoryDto.A,
            classSmallRepositoryDto.B + classSmallRepositoryDto.B
        );
    }
    
    public ClassMediumResponse GetClassMedium(ClassMediumRepositoryDto classMediumRepositoryDto)
    {
        return new ClassMediumResponse(
            classMediumRepositoryDto.A + classMediumRepositoryDto.A,
            classMediumRepositoryDto.B + classMediumRepositoryDto.B,
            classMediumRepositoryDto.X,
            classMediumRepositoryDto.Y
        );
    }
}


#endregion

#region DTOs

#region StructSmall
public struct StructSmallRequest
{
    public int A { get; }
    public int B { get; }

    public StructSmallRequest(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}
public struct StructSmallServiceDto
{
    public int A { get; }
    public int B { get; }

    public StructSmallServiceDto(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}
public struct StructSmallRepositoryDto
{
    public int A { get; }
    public int B { get; }

    public StructSmallRepositoryDto(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}
public struct StructSmallResponse
{
    public int A { get; }
    public int B { get; }

    public StructSmallResponse(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}

#endregion

#region StructMedium
public struct StructMediumRequest
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }
    
    public StructMediumRequest(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}
public struct StructMediumServiceDto
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }
    
    public StructMediumServiceDto(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}
public struct StructMediumRepositoryDto
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }
    
    public StructMediumRepositoryDto(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}
public struct StructMediumResponse
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }

    public StructMediumResponse(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}

#endregion

#region ClassSmall
public class ClassSmallRequest
{
    public int A { get; }
    public int B { get; }

    public ClassSmallRequest(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}
public class ClassSmallServiceDto
{
    public int A { get; }
    public int B { get; }

    public ClassSmallServiceDto(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}
public class ClassSmallRepositoryDto
{
    public int A { get; }
    public int B { get; }

    public ClassSmallRepositoryDto(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}

public class ClassSmallResponse
{
    public int A { get; }
    public int B { get; }

    public ClassSmallResponse(int A, int B)
    {
        this.A = A;
        this.B = B;
    }
}

#endregion

#region ClassMedium
public class ClassMediumRequest
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }
    
    public ClassMediumRequest(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}
public class ClassMediumServiceDto
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }
    
    public ClassMediumServiceDto(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}
public class ClassMediumRepositoryDto
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }
    
    public ClassMediumRepositoryDto(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}
public class ClassMediumResponse
{
    public int A { get; }
    public int B { get; }
    public string X { get; }
    public string Y { get; }

    public ClassMediumResponse(int A, int B, string X, string Y)
    {
        this.A = A;
        this.B = B;
        this.X = X;
        this.Y = Y;
    }
}

#endregion

#endregion
