using Entities;

namespace RESTAPI.Services;
public interface IAuthService
{
    Task<Entities.User> ValidateUser(string username, string password);
    Task RegisterUser(Entities.User user);
}
