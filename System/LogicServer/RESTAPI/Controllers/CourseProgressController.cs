using Microsoft.AspNetCore.Mvc;
using RepositoryContracts; 
using RESTAPI.Dtos;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseProgressController(ICourseProgressRepository repository) : ControllerBase
{
    // TODO: Only allow users to access their own progress unless they are admins

    // GET Progress 
    [HttpGet("{userId:int}/{courseId:int}")]
    [Authorize]
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
    [Authorize]
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