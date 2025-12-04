using Entities;

namespace RepositoryContracts;

public interface ILeaderboardRepository : IRepositoryID<LeaderboardEntry, LeaderboardEntry, LeaderboardEntry, string>
{
    Task<List<LeaderboardEntry>> GetTopPlayersAsync();
}