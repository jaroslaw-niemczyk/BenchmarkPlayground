using Microsoft.AspNetCore.Mvc;
using WebAppMvc.Dto;
using WebAppMvc.Entities;
using WebAppMvc.Services;

namespace WebAppMvc.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DtoBenchmarkController : ControllerBase
{
    private readonly Service _service;

    public DtoBenchmarkController(Service service)
    {
        _service = service;
    }

    [HttpPost(Name = "GetClassResponseDto")]
    public ClassEntity GetClassResponseDto(ClassRequestDto requestDto)
    {
        var serviceDto = new ClassServiceDto
        {
            Id = requestDto.Id,
            Name = requestDto.Name
        };
        
        return _service.HandleClassDto(serviceDto);
    }
    
    [HttpPost(Name = "GetStructResponseDto")]
    public StructEntity GetStructResponseDto(StructRequestDto requestDto)
    {
        var serviceDto = new StructServiceDto
        {
            Id = requestDto.Id,
            Name = requestDto.Name
        };
        
        return _service.HandleStructDto(serviceDto);
    }
    
    [HttpPost(Name = "GetRecordResponseDto")]
    public RecordEntity GetRecordResponseDto(RecordRequestDto requestDto)
    {
        var serviceDto = new RecordServiceDto(requestDto.Id, requestDto.Name);

        return _service.HandleRecordDto(serviceDto);
    }
}