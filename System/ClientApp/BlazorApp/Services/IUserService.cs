using System;
using BlazorApp.Entities;

namespace BlazorApp.Services;

public interface IUserService
{
    Task UpdateUser(int id, User User);
    public Task<List<User>> GetUsers();
    Task<User> GetUser(int id);
}
