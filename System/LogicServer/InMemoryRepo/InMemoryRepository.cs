using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryRepository<T, ID> : IRepositoryID<T, ID> where T : class, IIdentifiable<ID>
{
    private readonly List<T> Ts = [];

    public Task<T> AddAsync(T T)
    {
        // For composite keys, ID is set externally
        Ts.Add(T);
        return Task.FromResult(T);
    }

    public Task<T> UpdateAsync(T T)
    {
        T existingT =
            Ts.SingleOrDefault(p => p.Id.Equals(T.Id))
                ?? throw new NotFoundException($"T with ID '{T.Id}' not found");
        Ts.Remove(existingT);
        Ts.Add(T);
        return Task.FromResult(T);
    }

    public Task DeleteAsync(ID id)
    {
        T TToRemove =
            Ts.SingleOrDefault(p => p.Id.Equals(id))
                ?? throw new NotFoundException($"T with ID '{id}' not found");
        Ts.Remove(TToRemove);
        return Task.CompletedTask;
    }

    public Task<T> GetSingleAsync(ID id)
    {
        T T =
            Ts.SingleOrDefault(p => p.Id.Equals(id))
                ?? throw new NotFoundException($"T with ID '{id}' not found");
        return Task.FromResult(T);
    }

    public IQueryable<T> GetMany()
    {
        return Ts.AsQueryable();
    }

    public Task ClearAsync()
    {
        Ts.Clear();
        return Task.CompletedTask;
    }
}

public class InMemoryRepository<T> : InMemoryRepository<T, int> where T : class, IIdentifiable<int> { }
