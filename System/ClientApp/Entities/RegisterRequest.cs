using System;

namespace BlazorApp.Entities;

public class RegisterRequest
{
    public string Username { set; get; }
    public string Password { set; get; }
    public string PasswordRepeat { set; get; }
    public List<Role> Roles { set; get; }
}
