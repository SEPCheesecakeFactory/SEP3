using System;
using BlazorApp.Entities;
using BlazorApp.Shared;

namespace BlazorApp.Services;

public interface IUserService
{
    Task UpdateUser(int id, User User);
    public Task<Optional<List<User>>> GetUsers();
    Task<Optional<User>> GetUser(int id);
}
