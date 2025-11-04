using System;
using Entities;
using Grpc.Net.Client;
using RepositoryContracts;

namespace gRPCRepo;

public abstract class gRPCRepository<T> : IRepository<T> where T : class, IIdentifiable
{
    public DataRetrievalService.DataRetrievalServiceClient Client { protected get; init; }

    public gRPCRepository(string host, int port, bool useTls = false)
    {
        var channel = GrpcChannel.ForAddress($"http{(useTls ? "s" : "")}://{host}:{port}");
        Client = new DataRetrievalService.DataRetrievalServiceClient(channel);
    }
    public gRPCRepository(DataRetrievalService.DataRetrievalServiceClient client)
    {
        Client = client;
    }

    public abstract Task<T> AddAsync(T entity);
    public abstract Task ClearAsync();
    public abstract Task DeleteAsync(int id);
    public abstract IQueryable<T> GetMany();
    public abstract Task<T> GetSingleAsync(int id);
    public abstract Task UpdateAsync(T entity);
}
