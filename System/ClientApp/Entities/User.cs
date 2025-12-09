using System;

namespace BlazorApp.Entities;

public class User
{
    public int Id { set; get; }
    public string Username { set; get; }
    public string Password { set; get; }
    public List<Role> Roles { set; get; } = [];
}
