using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using Microsoft.Extensions.Logging;  // Add this

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRepository<Entities.User> userRepository;
    private readonly ILogger<AuthController> logger;  // Add this

    public AuthController(IRepository<Entities.User> userRepository, ILogger<AuthController> logger)  // Update constructor
    {
        this.userRepository = userRepository;
        this.logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        Entities.User? foundUser = await FindUserAsync(request.Username);
        if (foundUser == null)
        {
            logger.LogInformation("198273678126783"); 
            return Unauthorized();
        }
        if (request.Password != foundUser.Password)
        {
            return Unauthorized();
        }
        UserDto dto = new()
        {
            Id = foundUser.Id,
            Username = foundUser.Username
        };
        return dto;
    }

    private async Task<Entities.User?> FindUserAsync(string userName)
    {
        IEnumerable<Entities.User> users = userRepository.GetMany();
        logger.LogInformation("Finding user: {UserName}, Users count: {Count}", userName, users.Count());  // Add this
        foreach (Entities.User user in users)
        {
            logger.LogInformation("Checking user: {Username}", user.Username);  // Add this
            if (user.Username.Equals(userName))
            {
                logger.LogInformation("Found user: {Username}", user.Username);  // Add this
                return user;
            }
        }
        logger.LogInformation("User not found: {UserName}", userName);  // Add this
        return null;
    }
}