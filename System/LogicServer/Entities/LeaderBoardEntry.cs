namespace Entities;

public class LeaderboardEntry : IIdentifiable<string>
{
    public string Username { get; set; }
    public int TotalScore { get; set; }
    public int Rank { get; set; }
    public string Id { get => Username; set => Username = value; }
}