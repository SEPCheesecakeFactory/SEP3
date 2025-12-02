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
public class AuthController(IConfiguration config, IAuthService authService, IRepositoryID<Entities.User, Entities.User, Entities.User, int> userRepository, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IConfiguration _config = config;

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            logger.LogInformation("Login attempt for username: {Username}", request.Username);
            Entities.User foundUser = await authService.ValidateUser(request.Username, request.Password);
            logger.LogInformation("User validated: {Username}, Roles: {Roles}", foundUser.Username, string.Join(", ", foundUser.Roles.Select(r => r.RoleName)));
            string token = GenerateJwt(foundUser);
            logger.LogInformation("Token generated for {Username}", foundUser.Username);
            return Ok(token);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Login failed for {Username}", request.Username);
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
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            logger.LogInformation("Register attempt for username: {Username}", request.Username);
            Entities.User newUser = await authService.RegisterUser(request);



            string token = GenerateJwt(newUser);
            logger.LogInformation("Token generated for {Username}", newUser.Username);
            return Ok(token);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Register failed for {Username}", request.Username);
            return BadRequest(e.Message);
        }

    }


    private string GenerateJwt(Entities.User user)
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

    private List<Claim> GenerateClaims(Entities.User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Username", user.Username),
            new Claim("Id",user.Id.ToString())
            // new Claim("Email", user.Email),
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim("Role", role.RoleName));
        }

        return claims;
    }
}