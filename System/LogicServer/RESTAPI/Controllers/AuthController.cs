using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRepository<User> userRepository;
    public AuthController(IRepository<User> userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        
        User? foundUser = await FindUserAsync(request.Username);
        if (foundUser == null)
        {
            return Unauthorized();
        }
        if (request.Password!=foundUser.Password)
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
    private async Task<User?> FindUserAsync(string userName)
    {
        IEnumerable<User> users = userRepository.GetMany();
        foreach (User user in users)
        {
            if (user.Username.Equals(userName))
            {
                return user;
            }
        }
        return null;
        
    }
}
