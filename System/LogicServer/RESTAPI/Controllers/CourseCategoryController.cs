using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorApp.Entities;

namespace RESTAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class CourseCategoryController(ICourseCategoryRepository repository) : GenericController<CourseCategory, CreateCourseCategoryDto, CourseCategory, int>(repository)
{
    [HttpPost("/categories")]
    [Authorize("MustBeAdmin")]
    public async Task<ActionResult<CourseCategory>> CreateCategory([FromBody] CreateCourseCategoryDto entity) => await CreateAsync(entity);

    [HttpGet("/categories")]
    [Authorize]
    public ActionResult<IEnumerable<CourseCategory>> GetAll() => GetMany();

}