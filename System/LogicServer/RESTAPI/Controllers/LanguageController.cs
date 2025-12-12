using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorApp.Entities;

namespace RESTAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class LanguagesController(ILanguageRepository repository) : ControllerBase
{
    [HttpPost("/languages")] 
    [Authorize("MustBeAdmin")]
    public async Task<ActionResult<Language>> Create([FromBody] CreateLanguageDto dto)
    {
        try 
        {
            var created = await repository.AddAsync(dto);
            return Created($"/languages/{created.Code}", created);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("/languages")] 
    [Authorize]
    public ActionResult<IEnumerable<Language>> GetAll()
    {
        try 
        {
            var languages = repository.GetMany().ToList(); 
            return Ok(languages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}