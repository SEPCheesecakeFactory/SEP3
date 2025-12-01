using System;
using Entities;

namespace RepositoryContracts;

public interface IRepository<T> : IRepositoryID<T, int> where T : class { } 
