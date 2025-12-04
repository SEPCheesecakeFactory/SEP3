using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class CoursesController(ICourseRepository repository) : GenericController<Course, CreateCourseDto, Course, int>(repository)
{
    [HttpGet]
    public ActionResult<IEnumerable<Course>> HttpGetMany() => GetMany();

    [HttpPost]
    [Authorize("MustBeAdmin")]
    public async Task<ActionResult<Course>> HttpCreateAsync([FromBody] CreateCourseDto entity) => await CreateAsync(entity);

    [HttpGet("my-courses/{userId:int}")]
    public ActionResult<IEnumerable<Course>> HttpGetManyByUser(int userId)
    {
        var entities = repository.GetManyByUserId(userId);
        Console.WriteLine($"Fetched {entities.Count()} courses for user {userId}");
        return Ok(entities);
    }

    [HttpPut("{id}")]
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<Course>> HttpUpdateAsync(string id, [FromBody] Course entity) => await UpdateAsync(id, entity);
    
}