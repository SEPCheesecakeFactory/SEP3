using Entities;

namespace RepositoryContracts;

public interface ILeaderboardRepository
{
    Task<List<LeaderboardEntry>> GetTopPlayersAsync();
}