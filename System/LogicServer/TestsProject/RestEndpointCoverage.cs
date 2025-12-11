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
    private readonly HttpClient _client;
    private const string JwtTestKey = "super-secret-key-for-testing-only";
    private const string JwtIssuer = "test-issuer";
    private const string JwtAudience = "test-audience";
    private static string GenerateJwtToken(IEnumerable<string> roles) => TestingUtils.GenerateJwtToken(roles, JwtTestKey, JwtIssuer, JwtAudience);

    public RestEndpointCoverage(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Override JWT settings for testing
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = JwtTestKey,
                    ["Jwt:Issuer"] = JwtIssuer,
                    ["Jwt:Audience"] = JwtAudience
                });
            });
            builder.ConfigureServices(services =>
            {
                // Override services to use in-memory repos for testing
                services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
                services.AddSingleton<IRepositoryID<User, User, User, int>>(sp =>
                {
                    var repo = new InMemoryRepository<User, int>();
                    repo.AddAsync(new User { Id = 1, Username = "adminito", Password = "passwordini", Roles = [new() { RoleName = "admin" }] }).Wait();
                    repo.AddAsync(new User { Id = 2, Username = "teacherito", Password = "passwordini", Roles = [new() { RoleName = "teacher" }] }).Wait();
                    repo.AddAsync(new User { Id = 3, Username = "superuserito", Password = "passwordini", Roles = [new() { RoleName = "admin" }, new() { RoleName = "teacher" }] }).Wait();
                    repo.AddAsync(new User { Id = 4, Username = "userito", Password = "passwordini", Roles = [new() { RoleName = "learner" }] }).Wait();
                    return repo;
                });
                services.AddSingleton<IAuthService, AuthService>();
            });
        });
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CanReachServer()
    {
        var response = await _client.GetAsync("/status");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task FullAuthLifecycle()
    {
        await PureTests.AuthLifecycle(_client);
    }

    [Fact]
    public async Task FullCourseLifeCycle()
    {
        await PureTests.CourseLifeCycle(_client, GenerateJwtToken);
    }
}
