using System;

namespace BlazorApp.Entities;

// public class Course
// {
//     public int Id { set; get; }
//     public Language Language { set; get; }
//     public string Title { set; get; }
//     public string Descritpion { set; get; }
//     public CourseCategory Category{ set; get; }
    
    
// }
public class Course
{
    public int Id { get; set; }
    public string? Language { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public int TotalSteps { get; set; }
    public int? AuthorId { get; set; }
    public string? AuthorName { get; set; }
}
