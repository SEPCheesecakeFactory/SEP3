using System.ComponentModel.DataAnnotations;
using Entities;
using RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace RESTAPI.Services;

public class AuthService(IRepository<Entities.User> userRepository, ILogger<AuthService> logger) : IAuthService
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

    public Task RegisterUser(Entities.User user)
    {

        if (string.IsNullOrEmpty(user.Username))
        {
            throw new ValidationException("Username cannot be null");
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            throw new ValidationException("Password cannot be null");
        }
        // Do more user info validation here

        // save to persistence instead of list

        // users.Add(user); to be implemented

        return Task.CompletedTask;
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
