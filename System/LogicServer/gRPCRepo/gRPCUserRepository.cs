using Grpc.Net.Client;
using Entities;
using RepositoryContracts;

namespace gRPCRepo;

public class gRPCUserRepository : gRPCRepository<Entities.User>
{
    public gRPCUserRepository(string host, int port) : base(host, port)
    {
    }

    public override IQueryable<Entities.User> GetMany()
    {
        var resp = Client.GetUsers(new GetUsersRequest());
        var users = resp.Users.Select(c => new Entities.User
        {
            Id = c.Id,
            Username = c.Username,
            Password = c.Password,//this might have to be changed (we dont want to retrieve the list of all paswords xD ((just for now)))
            Role = c.Role,
        }).ToList();

        return users.AsQueryable();
    }

    public override Task<Entities.User> AddAsync(Entities.User entity)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(Entities.User entity)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task<Entities.User> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }

}
