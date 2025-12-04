namespace BlazorApp.Entities;

public class LeaderboardEntry
{
    public string Username { get; set; } = "";
    public int TotalScore { get; set; }
    public int Rank { get; set; }
}