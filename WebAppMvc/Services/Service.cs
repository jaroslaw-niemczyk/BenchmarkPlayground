using WebAppMvc.Dto;
using WebAppMvc.Entities;
using WebAppMvc.Repositories;

namespace WebAppMvc.Services;

public class Service
{
    private readonly Repository _repository;

    public Service(Repository repository)
    {
        _repository = repository;
    }

    public ClassEntity HandleClassDto(ClassServiceDto serviceDto)
    {
        var repositoryDto = new ClassRepositoryDto
        {
            Id = serviceDto.Id,
            Name = serviceDto.Name
        };

        return _repository.GetClassEntity(repositoryDto);
    }
    
    public StructEntity HandleStructDto(StructServiceDto serviceDto)
    {
        var repositoryDto = new StructRepositoryDto
        {
            Id = serviceDto.Id,
            Name = serviceDto.Name
        };

        return _repository.GetStructEntity(repositoryDto);
    }
    
    public RecordEntity HandleRecordDto(RecordServiceDto serviceDto)
    {
        var repositoryDto = new RecordRepositoryDto(serviceDto.Id, serviceDto.Name);

        return _repository.GetRecordEntity(repositoryDto);
    }
}