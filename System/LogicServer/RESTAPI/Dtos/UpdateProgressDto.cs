namespace RESTAPI.Dtos;

public class UpdateProgressDto
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int CurrentStep { get; set; }
}