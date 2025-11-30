using System;

namespace RepositoryContracts;

public interface IRepository<T> : IRepositoryID<T, int> where T : class { } 
