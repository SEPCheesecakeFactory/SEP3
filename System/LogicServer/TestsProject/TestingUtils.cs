using System;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace TestsProject;

public static class TestingUtils
{
    public static async Task<string> LoginAndGetToken(string username, string password, HttpClient client)
    {
        // using anonymous because 
        // 1. json doesn't care 
        // 2. no dependencies 
        // 3. simple 
        // 4. it should work this way; if it doesn't work, refactor the code - not the test
        var loginDto = new { Username = username, Password = password };

        var response = await client.PostAsJsonAsync("/Auth/login", loginDto);

        response.EnsureSuccessStatusCode();

        // Assuming the API returns 
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
}
