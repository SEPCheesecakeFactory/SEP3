using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class CoursesController(IRepositoryID<Course, CreateCourseDto, Course, int> repository) : GenericController<Course, CreateCourseDto, Course, int>(repository)
{
    [HttpGet]
    public ActionResult<IEnumerable<Course>> HttpGetMany() => GetMany();

    [HttpPost]
    [Authorize("MustBeAdmin")]
    public async Task<ActionResult<Course>> HttpCreateAsync([FromBody] CreateCourseDto entity) => await CreateAsync(entity);
}
