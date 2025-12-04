using System;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RESTAPI.Controllers;

public class GenericController<MainType, AddType, UpdateType, ID>(IRepositoryID<MainType, AddType, UpdateType, ID> repository) : ControllerBase where MainType : class
{
    protected readonly IRepositoryID<MainType, AddType, UpdateType, ID> _repository = repository;
    protected virtual Func<string, ID> IdParser { get; } = idStr => (ID)Convert.ChangeType(idStr, typeof(ID));

    protected IActionResult GetStatus()
    {
        _repository.GetMany(); // Dummy call to avoid unused field warning
        return Ok(new { status = $"Service is running" });
    }

    protected async Task<ActionResult<MainType>> GetSingleAsync(string id)
    {
        ID parsedId = IdParser(id);
        try
        {
            var entity = await _repository.GetSingleAsync(parsedId);
            return Ok(entity);
        }
        catch
        {
            return NotFound();
        }
    }

    protected ActionResult<IEnumerable<MainType>> GetMany()
    {
        var entities = _repository.GetMany();
        return Ok(entities);
    }

    protected async Task<ActionResult<MainType>> CreateAsync(AddType entity)
    {
        var createdEntity = await _repository.AddAsync(entity);
        return Created("", createdEntity);
    }

    protected async Task<ActionResult<MainType>> UpdateAsync(string id, UpdateType entity)
    {
        ID parsedId = IdParser(id);
        try
        {
            await _repository.UpdateAsync(entity);
            return Ok(_repository.GetSingleAsync(parsedId).Result);
        }
        catch
        {
            return NotFound();
        }
    }

    protected async Task<IActionResult> DeleteAsync(string id)
    {
        ID parsedId = IdParser(id);
        try
        {
            await _repository.DeleteAsync(parsedId);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
}
