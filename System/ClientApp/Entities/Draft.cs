namespace BlazorApp.Entities;

public class Draft
{
    public int Id { get; set; }
    public string? Language { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? TeacherId { get; set; }
    public int? CourseId { get; set; }
    public int? ApprovedBy { get; set; }
    public string? Category { get; set; }
    public string? AuthorName { get; set; }
}
