namespace RESTAPI.Dtos;

public class LearningStepDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CourseId { get; set; }
}