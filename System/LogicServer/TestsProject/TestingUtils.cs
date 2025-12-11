using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TestsProject;

public static class TestingUtils
{
    public static async Task<string> LoginAndGetToken(string username, string password, HttpClient client)
    {
        var loginDto = new { Username = username, Password = password };

        var response = await client.PostAsJsonAsync("/Auth/login", loginDto);

        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }

    public static (string, string) GetUserWithRoles(IEnumerable<string> roles)
    {
        if (!roles.Any())
            return ("userito", "passwordini");
        if (roles.Contains("admin"))
        {
            if (roles.Contains("teacher"))
                return ("superuserito", "passwordini");
            return ("adminito", "passwordini");
        }
        else if (roles.Contains("teacher"))
        {
            return ("teacherito", "passwordini");
        }
        else if (roles.Contains("learner"))
        {
            return ("userito", "passwordini");
        }
        throw new ArgumentException("No suitable user found for the given roles.");
    }

    
    public static async Task<string> GetTokenWithRoles(IEnumerable<string> roles, HttpClient client)
    {
        var (username, password) = GetUserWithRoles(roles);
        var tokenTask = LoginAndGetToken(username, password, client);
        tokenTask.Wait();
        return tokenTask.Result;
    }

    public static void Login(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    public static string GenerateJwtToken(IEnumerable<string> roles, string JwtTestKey, string JwtIssuer, string JwtAudience)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTestKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "testuser"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim("Role", role));
        }

        var token = new JwtSecurityToken(
            issuer: JwtIssuer,
            audience: JwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
