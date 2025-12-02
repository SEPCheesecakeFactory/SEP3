using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class DraftsController(IRepositoryID<Draft, CreateDraftDto, Draft, int> repository) : GenericController<Draft, CreateDraftDto, Draft, int>(repository)
{
    [HttpPost, Authorize("MustBeTeacher")]
    public async Task<ActionResult<Draft>> AddDraft([FromBody] CreateDraftDto dto)
    {
        try
        {
            var created = await _repository.AddAsync(dto);
            return Created($"/drafts/{created.Id}", created);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Draft>> GetDraft(int id) => await GetSingleAsync(id.ToString());
}
