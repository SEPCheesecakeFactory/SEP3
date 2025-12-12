using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using RESTAPI.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RESTAPI.Dtos;

namespace RESTAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            User foundUser = await authService.ValidateUser(request.Username, request.Password);
            string token = GenerateJwt(foundUser);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            User newUser = await authService.RegisterUser(request);
            string token = GenerateJwt(newUser);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        try
        {
            await authService.ChangePasswordAsync(request.Username, request.CurrentPassword, request.NewPassword);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private static string GenerateJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyString = "This Is My Random Secret Key Which Is At Least Sixteen Characters";
        Console.WriteLine($"JWT Key: '{keyString}'");
        var key = Encoding.UTF8.GetBytes(keyString);

        List<Claim> claims = GenerateClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "JWTAuthenticationServer",
            Audience = "JWTServiceBlazorClient"
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static List<Claim> GenerateClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new(ClaimTypes.Name, user.Username),
            new("Username", user.Username),
            new("Id",user.Id.ToString()),
            // new Claim("Email", user.Email),
            new("id", user.Id.ToString())
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new("Role", role.RoleName));
        }

        return claims;
    }
}