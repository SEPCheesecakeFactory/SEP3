namespace Entities;

public class LearningStep : IIdentifiable
{
    public int Id { get; set; }
    public LearningStepType? Type { get; set; }
    public LearningStepContent? Content { get; set; }
    public int CourseId { get; set; }
}
