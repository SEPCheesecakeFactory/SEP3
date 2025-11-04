using System;

namespace BlazorApp.Entities;

public class Course
{
    public int Id { set; get; }
    public Language Language { set; get; }
    public string Title { set; get; }
    public string Descritpion { set; get; }
    public CourseCategory Category{ set; get; }
    
}
