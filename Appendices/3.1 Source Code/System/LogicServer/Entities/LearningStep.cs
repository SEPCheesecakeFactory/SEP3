namespace Entities;

// Removed IIdentifiable because this entity has a Composite Key
public class LearningStep : IIdentifiable<(int, int)>
{
    public int CourseId { get; set; }
    public int StepOrder { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public (int, int) Id { get => (CourseId, StepOrder); set { CourseId = value.Item1; StepOrder = value.Item2; } }
}