using Microsoft.AspNetCore.Mvc;
using RepositoryContracts; 
using RESTAPI.Dtos;
using System.Threading.Tasks;
using System;

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseProgressController(ICourseProgressRepository repository) : ControllerBase
{
    // GET Progress 
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

    // POST Update
    [HttpPost]
    public async Task<ActionResult> UpdateProgress([FromBody] CourseProgressDto dto) 
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