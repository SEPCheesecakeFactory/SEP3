using Grpc.Net.Client;
using Entities;
using RepositoryContracts;
using System.Linq;
using via.sep3.dataserver.grpc;
using User = Entities.User;

namespace gRPCRepo;

public class gRPCUserRepository : gRPCRepository<User, User, User, int>
{
    public gRPCUserRepository(string host, int port) : base(host, port)
    {
    }

    public override IQueryable<User> GetMany()
    {
        var resp = Client.GetUsers(new GetUsersRequest());
        var users = resp.Users.Select(c => new User
        {
            Id = c.Id,
            Username = c.Username,
            Password = c.Password,//this might have to be changed (we dont want to retrieve the list of all paswords xD ((just for now)))
            Roles = c.Roles.Select(r => new Entities.Role { RoleName = r.Role_ }).ToList(),
        }).ToList();

        return users.AsQueryable();
    }

    public override async Task<Entities.User> AddAsync(Entities.User entity)
    {
        var request = new AddUserRequest
        {
            Username = entity.Username,
            Password = entity.Password,
            Roles = { entity.Roles.Select(r => r.RoleName) }
        };
        var response = await Client.AddUserAsync(request);
        return new Entities.User{
            Id = response.User.Id, // fix takes now id returned by database
            Username = entity.Username,
            Password = entity.Password,
            Roles = entity.Roles.Select(r => new Entities.Role { RoleName = r.RoleName }).ToList()
        };        
    }

    public override Task<Entities.User> UpdateAsync(Entities.User entity)
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
