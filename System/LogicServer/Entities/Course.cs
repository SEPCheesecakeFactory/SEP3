namespace Entities;

public class Course : IIdentifiable
{
    public int Id { get; set; }
    public char[]? LanguageCode { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
}
