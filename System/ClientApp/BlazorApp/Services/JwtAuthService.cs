using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;
using BlazorApp.Entities;

namespace BlazorApp.Services;

public class JwtAuthService(HttpClient client, IJSRuntime jsRuntime) : IAuthService
{
    // this private variable for simple caching
    public string Jwt { get; private set; } = "";

    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; } = null!;

    public async Task LoginAsync(string username, string password)
    {
        LoginRequest loginRequest = new()
        {
            Username = username,
            Password = password
        };

        string userAsJson = JsonSerializer.Serialize(loginRequest);
        StringContent content = new(userAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("auth/login", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }

        string token = responseContent;
        Jwt = token;

        await CacheTokenAsync();

        ClaimsPrincipal principal = await CreateClaimsPrincipal();

        OnAuthStateChanged.Invoke(principal);
    }

    private async Task<ClaimsPrincipal> CreateClaimsPrincipal()
    {
        var cachedToken = await GetTokenFromCacheAsync();
        if (string.IsNullOrEmpty(Jwt) && string.IsNullOrEmpty(cachedToken))
        {
            return new ClaimsPrincipal();
        }
        if (!string.IsNullOrEmpty(cachedToken))
        {
            Jwt = cachedToken;
        }
        if (!client.DefaultRequestHeaders.Contains("Authorization"))
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Jwt);

        IEnumerable<Claim> claims = ParseClaimsFromJwt(Jwt);

        ClaimsIdentity identity = new(claims, "jwt", "Username", "Role");

        ClaimsPrincipal principal = new(identity);
        return principal;
    }

    public async Task LogoutAsync()
    {
        await ClearTokenFromCacheAsync();
        client.DefaultRequestHeaders.Remove("Authorization");
        Jwt = "";
        ClaimsPrincipal principal = new();
        OnAuthStateChanged.Invoke(principal);
    }

    public async Task RegisterAsync(string userName, string password, string passwordRepeat, bool isTeacher)
    {
        var roles = new List<BlazorApp.Entities.Role>();
        roles.Add(new BlazorApp.Entities.Role { RoleName = "learner" });
        
        if (isTeacher)
        {
            roles.Add(new BlazorApp.Entities.Role { RoleName = "teacher" });
        }

        RegisterRequest registerRequest = new RegisterRequest
        {
            Username = userName,
            Password = password,
            PasswordRepeat = passwordRepeat,
            Roles = roles
        };
        string requestAsJson = JsonSerializer.Serialize(registerRequest);
        StringContent content = new(requestAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync("auth/register", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }
        string token = responseContent;
        Jwt = token;

        await CacheTokenAsync();

        ClaimsPrincipal principal = await CreateClaimsPrincipal();

        OnAuthStateChanged.Invoke(principal);
    }

    public async Task<ClaimsPrincipal> GetAuthAsync()
    {
        ClaimsPrincipal principal = await CreateClaimsPrincipal();
        return principal;
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        
        var claims = new List<Claim>();
        if (keyValuePairs == null) return claims;

        foreach (var kvp in keyValuePairs)
        {
            if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    claims.Add(new Claim(kvp.Key, item.ToString()));
                }
            }
            else
            {
                claims.Add(new Claim(kvp.Key, kvp.Value.ToString()!));
            }
        }
        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }

    private async Task<string?> GetTokenFromCacheAsync()
    {
        try
        {
            return await jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
        }
        catch
        {
            return null;
        }
    }

    private async Task CacheTokenAsync()
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwt", Jwt);
        }
        catch
        {
            // Ignore during prerendering
        }
    }

    private async Task ClearTokenFromCacheAsync()
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwt", "");
        }
        catch
        {
            // Ignore during prerendering
        }
    }
}