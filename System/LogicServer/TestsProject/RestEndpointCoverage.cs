using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RepositoryContracts;
using RESTAPI;
using RESTAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;
using InMemoryRepositories;

namespace TestsProject;

public class RestEndpointCoverage : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RestEndpointCoverage(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Override JWT settings for testing
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "super-secret-key-for-testing-only",
                    ["Jwt:Issuer"] = "test-issuer",
                    ["Jwt:Audience"] = "test-audience"
                });
            });
            builder.ConfigureServices(services =>
            {
                // Override services to use in-memory repos for testing
                services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
                services.AddSingleton<IRepositoryID<User, User, User, int>>(sp =>
                {
                    var repo = new InMemoryRepository<User, int>();
                    repo.AddAsync(new User { Id = 1, Username = "adminito", Password = "passwordini", Roles = [new() { RoleName = "Admin" }] }).Wait();
                    repo.AddAsync(new User { Id = 2, Username = "teacherito", Password = "passwordini", Roles = [new() { RoleName = "Teacher" }] }).Wait();
                    repo.AddAsync(new User { Id = 3, Username = "superuserito", Password = "passwordini", Roles = [new() { RoleName = "Admin" }, new() { RoleName = "Teacher" }] }).Wait();
                    repo.AddAsync(new User { Id = 4, Username = "userito", Password = "passwordini", Roles = [new() { RoleName = "Learner" }] }).Wait();
                    return repo;
                });
                services.AddSingleton<IAuthService, AuthService>();
            });
        });
    }

    private string GenerateJwtToken(IEnumerable<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-for-testing-only"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "testuser"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: "test-issuer",
            audience: "test-audience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Fact]
    public async Task TestAuthEndpoint()
    {
        var client = _factory.CreateClient();

        var loginDto = new { Username = "adminito", Password = "passwordini" };
        var response = await client.PostAsJsonAsync("/Auth/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestCourseEndpoints()
    {
        var client = _factory.CreateClient();

        // Test GET /courses (requires auth)
        var token = GenerateJwtToken(["learner"]);
        client.Login(token);

        var response = await client.GetAsync("/courses");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test POST /drafts (create course)
        token = GenerateJwtToken(["teacher"]);
        client.Login(token);

        var createDto = new CreateCourseDto
        {
            Language = "ENG",
            Title = "Test Course",
            Description = "Test Description",
            Category = "History",
            AuthorId = 1
        };
        var createResponse = await client.PostAsJsonAsync("/drafts", createDto);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCourse = await createResponse.Content.ReadFromJsonAsync<Course>();
        createdCourse.Should().NotBeNull();
        createdCourse!.Title.Should().Be("Test Course");

        // Test GET /courses/my-courses/{userId}
        var myCoursesResponse = await client.GetAsync("/courses/my-courses/1");
        myCoursesResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test PUT /courses/{id} (update)
        createdCourse.Description = "Updated Description";
        var updateResponse = await client.PutAsJsonAsync($"/courses/{createdCourse.Id}", createdCourse);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test PUT /drafts/{id} (approve draft) - requires admin
        var adminToken = GenerateJwtToken(["admin"]);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var approveResponse = await client.PutAsJsonAsync($"/drafts/{createdCourse.Id}", 1); // approvedBy = 1
        approveResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
