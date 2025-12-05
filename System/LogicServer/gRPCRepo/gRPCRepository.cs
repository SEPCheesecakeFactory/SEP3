using System;
using Entities;
using Grpc.Net.Client;
using RepositoryContracts;
using via.sep3.dataserver.grpc;

namespace gRPCRepo;

public abstract class gRPCRepository<T, TAdd, TUpdate, ID> : IRepositoryID<T, TAdd, TUpdate, ID> where T : class, IIdentifiable<ID>
{
    public UserService.UserServiceClient UserServiceClient { protected get; init; }
    public CourseService.CourseServiceClient CourseServiceClient { protected get; init; }
    public ProgressService.ProgressServiceClient ProgressServiceClient { protected get; init; }

    public gRPCRepository(string host, int port, bool useTls = false)
    {
        var channel = GrpcChannel.ForAddress($"http{(useTls ? "s" : "")}://{host}:{port}");
        UserServiceClient = new UserService.UserServiceClient(channel);
        CourseServiceClient = new CourseService.CourseServiceClient(channel);
        ProgressServiceClient = new ProgressService.ProgressServiceClient(channel);
    }
    public gRPCRepository(UserService.UserServiceClient UserServiceClient, CourseService.CourseServiceClient CourseServiceClient, ProgressService.ProgressServiceClient ProgressServiceClient)
    {
        this.UserServiceClient = UserServiceClient;
        this.CourseServiceClient=CourseServiceClient;
        this.ProgressServiceClient = ProgressServiceClient;
    }

    public abstract Task ClearAsync();
    public abstract Task DeleteAsync(ID id);
    public abstract IQueryable<T> GetMany();
    public abstract Task<T> GetSingleAsync(ID id);
    public abstract Task<T> AddAsync(TAdd entity);
    public abstract Task<T> UpdateAsync(TUpdate entity);
}

public abstract class gRPCRepository<T, ID> : gRPCRepository<T, T, T, ID> where T : class, IIdentifiable<ID>
{
    public gRPCRepository(string host, int port, bool useTls = false) : base(host, port, useTls) { }
    // public gRPCRepository(DataRetrievalService.DataRetrievalServiceClient client) : base(client) { }
     public gRPCRepository(UserService.UserServiceClient UserServiceClient, CourseService.CourseServiceClient CourseServiceClient, ProgressService.ProgressServiceClient ProgressServiceClient) : base(UserServiceClient, CourseServiceClient, ProgressServiceClient)
    {
        
    }
}
