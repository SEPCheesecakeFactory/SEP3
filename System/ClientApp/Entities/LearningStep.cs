namespace BlazorApp.Entities;

public class LearningStep
{
    public int CourseId { get; set; }
    public int StepOrder { get; set; }
    public string Type { get; set; } = "";
    public string Content { get; set; } = "";

    public LearningStep DeepCopy()
    {
        return new LearningStep
        {
            CourseId = this.CourseId,
            StepOrder = this.StepOrder,
            Type = this.Type,
            Content = this.Content
        };
    }

    public static bool operator ==(LearningStep? left, LearningStep? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        return left.CourseId == right.CourseId &&
               left.StepOrder == right.StepOrder &&
               left.Type == right.Type &&
               left.Content == right.Content;
    }

    public static bool operator !=(LearningStep? left, LearningStep? right)
    {
        return !(left == right);
    }
}
