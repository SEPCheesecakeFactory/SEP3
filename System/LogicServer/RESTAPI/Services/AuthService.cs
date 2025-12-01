using System.ComponentModel.DataAnnotations;
using Entities;
using RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace RESTAPI.Services;

public class AuthService(IRepositoryID<Entities.User, int> userRepository, ILogger<AuthService> logger) : IAuthService
{
    public async Task<Entities.User> ValidateUser(string username, string password)
    {
        Entities.User existingUser = await FindUserAsync(username);
        if (!existingUser.Password.Equals(password))
        {
            throw new Exception("Password mismatch");
        }

        return existingUser;
    }

    public async Task<Entities.User> RegisterUser(RegisterRequest request)
    {

        if (string.IsNullOrEmpty(request.Username))
        {
            throw new ValidationException("Username cannot be null");
        }
        try
        {
            await FindUserAsync(request.Username);
            throw new ValidationException("Username already exists!");
        }
        catch (Exception e) when (e.Message.Contains("User not found"))
        {
            // User not found, so registration can proceed
        }
        if (string.IsNullOrEmpty(request.Password))
        {
            throw new ValidationException("Password cannot be null");
        }
        if (request.Password != request.PasswordRepeat)
        {
            throw new ValidationException("Passwords are not the same!");
        }
        if (request.Roles == null || request.Roles.Count == 0)
        {
            throw new ValidationException("No roles assigned");
        }
        // more user info validation here

        
        Entities.User userNoId = new Entities.User
        {
            Username = request.Username,
            Password = request.Password,
            Roles = request.Roles
        };
        Entities.User newUser = await userRepository.AddAsync(userNoId);

        return newUser;
    }
    private async Task<Entities.User> FindUserAsync(string userName)
    {
        IEnumerable<Entities.User> users = await Task.Run(() => userRepository.GetMany());
        logger.LogInformation("Finding user: {UserName}, Users count: {Count}", userName, users.Count());
        foreach (Entities.User user in users)
        {
            logger.LogInformation("Checking user: {Username}", user.Username);
            if (user.Username.Equals(userName))
            {
                logger.LogInformation("Found user: {Username}", user.Username);
                return user;
            }
        }
        logger.LogInformation("User not found: {UserName}", userName);
        throw new Exception("User not found");
    }


}
