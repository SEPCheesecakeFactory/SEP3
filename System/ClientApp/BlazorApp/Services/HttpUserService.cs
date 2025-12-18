using System;
using System.Text;
using System.Text.Json;
using BlazorApp.Entities;
using System.Net.Http.Json;
namespace BlazorApp.Services;

using BlazorApp.Entities;
using BlazorApp.Shared;

public class HttpUserService(HttpCrudService service) : IUserService
{
    public async Task<Optional<List<User>>> GetUsers()
        => await service.GetAsync<List<User>>("users") ?? Optional<List<User>>.Empty();

    public async Task UpdateUser(int id, User user)
        => await service.UpdateAsync<User, User>($"users/{id}", user);

    public async Task<Optional<User>> GetUser(int id)
        => await service.GetAsync<User>($"users/{id}");
}
