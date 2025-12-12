namespace BlazorApp.Entities;

public class CreateDraftDto
{
    public string? Language { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? TeacherId { get; set; }
    public string? Category { get; set; }
    public int? AuthorId { get; set; }
}
