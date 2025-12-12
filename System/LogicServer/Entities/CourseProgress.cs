
using System;
namespace Entities; 

public class CourseProgress : IIdentifiable<string>
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int CurrentStep { get; set; }

    public string Id
    {
        get => $"{UserId}_{CourseId}"; 
        set {}
    }
}