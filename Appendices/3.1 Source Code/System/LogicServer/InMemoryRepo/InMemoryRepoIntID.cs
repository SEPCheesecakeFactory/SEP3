using System;
using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryRepoIntID<T> : InMemoryRepository<T, int> where T : class, IIdentifiable<int>
{
    public override Task<T> AddAsync(T T)
    {
        // Assign a new ID
        int newId = Ts.Count == 0 ? 1 : Ts.Max(e => e.Id) + 1;
        T.Id = newId;
        return base.AddAsync(T);
    }
}
