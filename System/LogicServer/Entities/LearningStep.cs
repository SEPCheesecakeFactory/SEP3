namespace Entities;

public class LearningStep : IIdentifiable
{
public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CourseId { get; set; }}
