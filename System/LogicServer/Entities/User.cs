using System;

namespace Entities;

public class User : IIdentifiable
{
    public int Id { get; set; }
    public string Username { set; get; }
    public string Password { set; get; }
    public List<Role> Roles { set; get; }
}
