using WebAppMvc.Dto;
using WebAppMvc.Entities;

namespace WebAppMvc.Repositories;

public class Repository
{
    public ClassEntity GetClassEntity(ClassRepositoryDto repositoryDto)
    {
        return new ClassEntity
        {
            String = repositoryDto.Name,
            IntId = repositoryDto.Id + repositoryDto.Id
        };
    }

    public StructEntity GetStructEntity(StructRepositoryDto repositoryDto)
    {
        return new StructEntity
        {
            String = repositoryDto.Name,
            IntId = repositoryDto.Id + repositoryDto.Id
        };
    }
    
    public RecordEntity GetRecordEntity(RecordRepositoryDto repositoryDto)
    {
        return new RecordEntity(repositoryDto.Id + repositoryDto.Id, repositoryDto.Name);
    }
}