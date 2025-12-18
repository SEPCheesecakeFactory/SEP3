using Entities;

namespace RESTAPI.Services;

public interface IAuthService
{
    Task<User> ValidateUser(string username, string password);
    Task<User> RegisterUser(RegisterRequest request);
    Task ChangePasswordAsync(string username, string currentPassword, string newPassword);
}
