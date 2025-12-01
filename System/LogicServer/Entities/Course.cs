namespace Entities;

public class Course : IIdentifiable<int>
{
    public int Id { get; set; }
    public string? Language { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
}
