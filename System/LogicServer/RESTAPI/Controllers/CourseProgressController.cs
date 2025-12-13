using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using RESTAPI.Dtos;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseProgressController(ICourseProgressRepository repository) : ControllerBase
{
    // GET Progress 
    [HttpGet("{userId:int}/{courseId:int}")]
    [Authorize]
    public async Task<ActionResult<int>> GetProgress(int userId, int courseId)
    {
        int step = await repository.GetCourseProgressAsync(userId, courseId);
        return Ok(step);
    }

    // POST Update
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> UpdateProgress([FromBody] CourseProgressDto dto)
    {
        var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
        if (idClaim == null || !int.TryParse(idClaim.Value, out int authenticatedUserId) || authenticatedUserId != dto.UserId)
        {
            return Forbid();
        }
        var current = await repository.UpdateCourseProgressAsync(dto.UserId, dto.CourseId, dto.CurrentStep);
        return Ok(current);
    }
    // GET /CourseProgress/{userId}/{courseId}
    [HttpDelete("{courseId:int}/{userId:int}")]
    public async Task<ActionResult> DeleteProgress(int courseId, int userId)
    {
        try
        {        
        await repository.DeleteAsync(courseId, userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
        return NoContent();
    }
}