using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseGrpcController<TEntity, TReadDto, TCreateDto> : ControllerBase 
    where TEntity : class 
    where TReadDto : class 
    where TCreateDto : class
{
    protected readonly IRepository<TEntity> _repository;

    public BaseGrpcController(IRepository<TEntity> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TReadDto>>> GetAll()
    {
        var entities = await Task.Run(() => _repository.GetMany());
        // We delegate mapping to the concrete controller to avoid AutoMapper dependency for now
        var dtos = MapToDto(entities); 
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TReadDto>> Get(int id)
    {
        var entity = await _repository.GetSingleAsync(id);
        if (entity == null) return NotFound();
        return Ok(MapToDto(entity));
    }

    [HttpPost]
    public virtual async Task<ActionResult<TReadDto>> Add([FromBody] TCreateDto createDto)
    {
        var entity = MapToEntity(createDto);
        var createdEntity = await _repository.AddAsync(entity);
        return Ok(MapToDto(createdEntity));
    }

    // Abstract mapping methods - you implement these in the specific controller
    protected abstract TReadDto MapToDto(TEntity entity);
    protected abstract IEnumerable<TReadDto> MapToDto(IEnumerable<TEntity> entities);
    protected abstract TEntity MapToEntity(TCreateDto dto);
}