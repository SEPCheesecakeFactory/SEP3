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
        var result = await repository.GetTopPlayersAsync();
        return Ok(result);
    }
}