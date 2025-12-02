using System;
using Entities;
using Grpc.Net.Client;
using RepositoryContracts;
using via.sep3.dataserver.grpc;

namespace gRPCRepo;

public abstract class gRPCRepository<T, TAdd, TUpdate, ID> : IRepositoryID<T, TAdd, TUpdate, ID> where T : class, IIdentifiable<ID>
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

    public abstract Task ClearAsync();
    public abstract Task DeleteAsync(ID id);
    public abstract IQueryable<T> GetMany();
    public abstract Task<T> GetSingleAsync(ID id);
    public abstract Task<T> AddAsync(TAdd entity);
    public abstract Task UpdateAsync(TUpdate entity);
}

public abstract class gRPCRepository<T, ID> : gRPCRepository<T, T, T, ID> where T : class, IIdentifiable<ID>
{
    public gRPCRepository(string host, int port, bool useTls = false) : base(host, port, useTls) { }
    public gRPCRepository(DataRetrievalService.DataRetrievalServiceClient client) : base(client) { }
}
