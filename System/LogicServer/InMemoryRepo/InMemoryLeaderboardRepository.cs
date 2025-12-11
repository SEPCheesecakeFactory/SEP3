using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryLeaderboardRepository : ILeaderboardRepository
{
    private readonly List<LeaderboardEntry> entries = [];

    public Task<LeaderboardEntry> AddAsync(LeaderboardEntry entry)
    {
        entries.Add(entry);
        return Task.FromResult(entry);
    }

    public Task<LeaderboardEntry> UpdateAsync(LeaderboardEntry entry)
    {
        var existing = entries.SingleOrDefault(e => e.Id == entry.Id)
            ?? throw new NotFoundException($"LeaderboardEntry with ID '{entry.Id}' not found");
        entries.Remove(existing);
        entries.Add(entry);
        return Task.FromResult(entry);
    }

    public Task DeleteAsync(string id)
    {
        var entry = entries.SingleOrDefault(e => e.Id == id)
            ?? throw new NotFoundException($"LeaderboardEntry with ID '{id}' not found");
        entries.Remove(entry);
        return Task.CompletedTask;
    }

    public Task<LeaderboardEntry> GetSingleAsync(string id)
    {
        var entry = entries.SingleOrDefault(e => e.Id == id)
            ?? throw new NotFoundException($"LeaderboardEntry with ID '{id}' not found");
        return Task.FromResult(entry);
    }

    public IQueryable<LeaderboardEntry> GetMany()
    {
        return entries.AsQueryable();
    }

    public Task ClearAsync()
    {
        entries.Clear();
        return Task.CompletedTask;
    }

    public Task<List<LeaderboardEntry>> GetTopPlayersAsync()
    {
        return Task.FromResult(entries.OrderByDescending(e => e.TotalScore).Take(10).ToList());
    }
}