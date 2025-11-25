namespace Entities;

// Removed IIdentifiable because this entity has a Composite Key
public class LearningStep 
{
    public int CourseId { get; set; }
    public int StepOrder { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}