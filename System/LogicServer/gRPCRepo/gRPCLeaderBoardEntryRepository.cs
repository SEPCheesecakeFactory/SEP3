using System;
using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;
using LeaderboardEntry = Entities.LeaderboardEntry;

namespace gRPCRepo;

public class gRPCLeaderBoardEntryRepository(string host, int port) : gRPCRepository<LeaderboardEntry, LeaderboardEntry, LeaderboardEntry, string>(host, port), ILeaderboardRepository
{
    public override Task<LeaderboardEntry> AddAsync(LeaderboardEntry entity)
    {
        throw new NotImplementedException();
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public override IQueryable<LeaderboardEntry> GetMany()
    {
        throw new NotImplementedException();
    }

    public override Task<LeaderboardEntry> GetSingleAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Entities.LeaderboardEntry>> GetTopPlayersAsync()
    {
        // 1. Call Java (gRPC)
        var response = await Client.GetLeaderboardAsync(new Empty());

        // 2. Map Proto -> C# Entity
        return [.. response.Entries.Select(e => new Entities.LeaderboardEntry
        {
            Username = e.Username,
            TotalScore = e.TotalScore,
            Rank = e.Rank
        })];
    }

    public override Task<LeaderboardEntry> UpdateAsync(LeaderboardEntry entity)
    {
        throw new NotImplementedException();
    }
}
