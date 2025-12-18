namespace Entities;

public class CreateCourseDto
{
    public string? Language { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category{set;get;}
    public int? AuthorId { get; set; }

}
