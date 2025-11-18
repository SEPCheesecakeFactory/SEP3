using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using Microsoft.Extensions.Logging;
using RESTAPI.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IConfiguration config, IAuthService authService, IRepository<Entities.User> userRepository, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IConfiguration _config = config;

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            Entities.User foundUser = await authService.ValidateUser(request.Username, request.Password);
            string token = GenerateJwt(foundUser);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        //old implementation without jwt token
        // if (foundUser == null)
        // {
        //     logger.LogInformation("198273678126783");
        //     return Unauthorized();
        // }
        // if (request.Password != foundUser.Password)
        // {
        //     return Unauthorized();
        // }
        // UserDto dto = new()
        // {
        //     Id = foundUser.Id,
        //     Username = foundUser.Username
        // };
        // return dto;
    }


    private string GenerateJwt(Entities.User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(config["Jwt:Key"] ?? "");

        List<Claim> claims = GenerateClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private List<Claim> GenerateClaims(Entities.User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"] ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("Username", user.Username),
            new Claim("Role", user.Role),
            // new Claim("Email", user.Email),
        };
        return [.. claims];
    }
}