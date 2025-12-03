using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Threading.Tasks;
using System;

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseProgressController(ICourseRepository repository) : ControllerBase
{
    // GET Progress
    // GET /CourseProgress/{userId}/{courseId}
    [HttpGet("{userId:int}/{courseId:int}")]
    public async Task<ActionResult<int>> GetProgress(int userId, int courseId)
    {
        try
        {
            int step = await repository.GetCourseProgressAsync(userId, courseId);
            return Ok(step);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // POST (Update) Progress
    // POST /CourseProgress
    // { "userId": 1, "courseId": 2, "currentStep": 5 }
    [HttpPost]
    public async Task<ActionResult> UpdateProgress([FromBody] UpdateProgressDto dto)
    {
        try
        {
            await repository.UpdateCourseProgressAsync(dto.UserId, dto.CourseId, dto.CurrentStep);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}

//  DTO 
public class UpdateProgressDto
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int CurrentStep { get; set; }
}