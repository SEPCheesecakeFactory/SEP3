using System;

namespace Entities;

public class Language : IIdentifiable<string>{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Id 
    { 
        get => Code; 
        set => Code = value; 
    }
}