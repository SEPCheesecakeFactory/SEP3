using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorApp.Entities;

namespace RESTAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class LanguagesController(IRepositoryID<Language, CreateLanguageDto, Language, string> languageRepository) : GenericController<Language, CreateLanguageDto, Language, string>(languageRepository)
{
    [HttpPost("/languages")] 
    [Authorize("MustBeAdmin")]
    public async Task<ActionResult<Language>> Create([FromBody] CreateLanguageDto dto) => await CreateAsync(dto);

    [HttpGet("/languages")] 
    [Authorize]
    public ActionResult<IEnumerable<Language>> GetAll() => GetMany();
}