using System;
using System.Text;
using System.Text.Json;
using BlazorApp.Entities;
using System.Net.Http.Json;
namespace BlazorApp.Services;

using BlazorApp.Entities;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;
    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<List<User>> GetUsers()
    {
        var result = await client.GetFromJsonAsync<List<User>>("users");
        return new List<User>(result ?? new List<User>());
    }

    public async Task UpdateUser(int id, User User)
    {
        var json = JsonSerializer.Serialize(User);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"users/{id}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }


}
