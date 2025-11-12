using System;

namespace Entities;

public class User : IIdentifiable
{
    public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Username { set; get; }
    public string Password { set; get; }
    public string Role { set; get; }
}
