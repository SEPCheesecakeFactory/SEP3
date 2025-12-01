namespace BlazorApp.Entities;

public class LearningStep
{
    public int CourseId { get; set; }
    public int StepOrder { get; set; }
    public string Type { get; set; } = "";
    public string Content { get; set; } = "";
}
