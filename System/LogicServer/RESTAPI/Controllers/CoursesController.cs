using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorApp.Entities;

namespace RESTAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class CoursesController(ICourseRepository repository) : GenericController<Course, CreateCourseDto, Course, int>(repository)
{
    [HttpGet]
    public ActionResult<IEnumerable<Course>> HttpGetMany() => GetMany();

    [HttpPost("/drafts")]//so that we dont need to change at least this route in blazor
    [Authorize("MustBeTeacher")]
    public async Task<ActionResult<Course>> HttpCreateAsync([FromBody] CreateCourseDto entity) => await CreateAsync(entity);


    [HttpGet("/drafts")]
    [Authorize("MustBeTeacherOrAdmin")]
    public ActionResult<IEnumerable<Course>> GetDrafts()
    {
        try
        {
            // get everything
            var allCourses = repository.GetMany();

            // filter for unapproved (0 or null)
            var drafts = allCourses.Where(c => c.ApprovedBy == null || c.ApprovedBy == 0);

            return Ok(drafts);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("my-courses/{userId:int}")]
    public ActionResult<IEnumerable<Course>> HttpGetManyByUser(int userId)
    {
        var entities = repository.GetManyByUserId(userId);
        Console.WriteLine($"Fetched {entities.Count()} courses for user {userId}");
        return Ok(entities);
    }

    [HttpPut("{id}")]
    [Authorize("MustBeTeacherOrAdmin")]
    public async Task<ActionResult<Course>> HttpUpdateAsync(string id, [FromBody] Course entity) => await UpdateAsync(id, entity);

    [HttpPut("/drafts/{id:int}"), Authorize("MustBeAdmin")]
    public async Task<ActionResult<Course>> ApproveDraft([FromBody] int approvedBy, [FromRoute] int id)
    {
        try
        {
            Course currentCourse = await _repository.GetSingleAsync(id);
            Course updatedCourse = new()
            {
                Id = currentCourse.Id,
                Language = currentCourse.Language,
                Title = currentCourse.Title,
                Description = currentCourse.Description,
                AuthorId = currentCourse.AuthorId,
                ApprovedBy = approvedBy,
                TotalSteps = currentCourse.TotalSteps //added this because i dont know at what point we are changing this value yet
            };

            currentCourse = await _repository.UpdateAsync(updatedCourse);
            return Ok(updatedCourse);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}