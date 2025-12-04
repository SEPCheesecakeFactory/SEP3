using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LeaderboardController(ILeaderboardRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LeaderboardEntry>>> GetLeaderboard()
    {
        try
        {
            var result = await repository.GetTopPlayersAsync();
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}