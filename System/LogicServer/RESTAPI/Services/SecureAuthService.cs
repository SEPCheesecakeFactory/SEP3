using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using Isopoh.Cryptography.Argon2;
using RepositoryContracts;

namespace RESTAPI.Services;

public class SecureAuthService(IRepositoryID<User, int> userRepository) : IAuthService
{
    public async Task<User> RegisterUser(RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Username))
            throw new ValidationException("Username cannot be null");
        if (string.IsNullOrEmpty(request.Password))
            throw new ValidationException("Password cannot be null");
        if (request.Password != request.PasswordRepeat)
            throw new ValidationException("Passwords are not the same!");
        if (request.Roles == null || request.Roles.Count == 0)
            throw new ValidationException("No roles assigned");

        var user = await GetUserByUsernameAsync(request.Username);

        if (user != null)
            throw new ValidationException("Username already exists!");

        var passwordHash = Argon2.Hash(request.Password);
        
        User userNoId = new()
        {
            Username = request.Username,
            Password = passwordHash,
            Roles = request.Roles
        };
        User newUser = await userRepository.AddAsync(userNoId);
        return newUser;
    }

    public async Task<User> ValidateUser(string username, string password)
    {
        User? existingUser = await GetUserByUsernameAsync(username) ?? throw new Exception("User not found");
        
        if (!Argon2.Verify(existingUser.Password, password))
            throw new Exception("Password mismatch");

        return existingUser;
    }

    private async Task<User?> GetUserByUsernameAsync(string userName)
    {
        IEnumerable<User> users = await Task.Run(() => userRepository.GetMany());
        foreach (User user in users)
        {
            if (user.Username.Equals(userName))
            {
                return user;
            }
        }
        return null;
    }
}
