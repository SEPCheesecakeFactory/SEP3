namespace Entities;

public class learningStep : IIdentifiable
{
    public int Id { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; } //Probably a list of options
}
