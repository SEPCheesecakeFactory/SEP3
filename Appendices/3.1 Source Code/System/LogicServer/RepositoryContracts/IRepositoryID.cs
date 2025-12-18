using System;

namespace RepositoryContracts;

public interface IRepositoryID<T, TAdd, TUpdate, ID> where T : class
{
    Task<T> AddAsync(TAdd entity); // Takes an entity and returns the created entity
    Task<T> UpdateAsync(TUpdate entity); // Takes an entity and replaces it by ID or throws an exception if ID not present
    Task DeleteAsync(ID id); // Takes an ID and deletes the entity or throws an exception if ID not present
    Task<T> GetSingleAsync(ID id); // Takes an ID and returns the entity or throws an exception if ID not present
    IQueryable<T> GetMany(); // Returns all entities as IQueryable
    Task ClearAsync();
}

public interface IRepositoryID<T, ID> : IRepositoryID<T, T, T, ID> where T : class { }
public interface IRepositoryID<T> : IRepositoryID<T, int> where T : class { }
