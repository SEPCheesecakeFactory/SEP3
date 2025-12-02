using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers;

[Authorize]
public class DraftsController(IRepositoryID<Draft, int> repository) : GenericController<Draft, int>(repository)
{
    [HttpPost, Authorize("MustBeTeacher")]
    public async Task<ActionResult<Draft>> AddDraft([FromBody] CreateDraftDto dto)
    {
        try
        {
            Draft draft = new()
            {
                Language = dto.Language,
                Title = dto.Title,
                Description = dto.Description,
                TeacherId = dto.TeacherId
            };
            Draft created = await repository.AddAsync2(draft);
            return Created($"/drafts/{created.Id}", created);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Draft>> GetDraft(int id)
    {
        try
        {
            Draft draft = await repository.GetSingleAsync(id);
            return Ok(draft);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
