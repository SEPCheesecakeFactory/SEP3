using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http; // Fixed missing using
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; // REQUIRED for RemoveAll
using RepositoryContracts;
using RESTAPI;
using RESTAPI.Services;
using Xunit;
using InMemoryRepositories;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace TestsProject;

public class RestEndpointCoverage : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private const string JwtTestKey = "super-secret-key-for-testing-only";
    private const string JwtIssuer = "test-issuer";
    private const string JwtAudience = "test-audience";
    private static string GenerateJwtToken(IEnumerable<string> roles) => TestingUtils.GenerateJwtToken(roles, JwtTestKey, JwtIssuer, JwtAudience);
    private readonly ITestOutputHelper _testOutputHelper;

    public RestEndpointCoverage(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = JwtTestKey,
                    ["Jwt:Issuer"] = JwtIssuer,
                    ["Jwt:Audience"] = JwtAudience
                });
            });

            builder.ConfigureServices(services =>
            {
                // 1. Remove ALL existing gRPC registrations to prevent accidental resolution
                services.RemoveAll<ICourseRepository>();
                services.RemoveAll<IRepositoryID<Course, CreateCourseDto, Course, int>>();
                services.RemoveAll<IRepositoryID<User, User, User, int>>();
                services.RemoveAll<IAuthService>();
                services.RemoveAll<ILeaderboardRepository>();
                services.RemoveAll<ICourseProgressRepository>();
                services.RemoveAll<IRepositoryID<LearningStep, LearningStep, LearningStep, (int, int)>>();

                // 2. Create the InMemory instances                
                var userRepo = new InMemoryRepoIntID<User>();
                var learningStepRepo = new InMemoryRepository<LearningStep, (int, int)>();
                var courseRepo = new InMemoryCourseRepository(learningStepRepo);

                // Add fake user data
                userRepo.AddAsync(new User { Id = 1, Username = "adminito", Password = "passwordini", Roles = [new() { RoleName = "admin" }] }).Wait();
                userRepo.AddAsync(new User { Id = 2, Username = "teacherito", Password = "passwordini", Roles = [new() { RoleName = "teacher" }] }).Wait();
                userRepo.AddAsync(new User { Id = 3, Username = "superuserito", Password = "passwordini", Roles = [new() { RoleName = "admin" }, new() { RoleName = "teacher" }] }).Wait();
                userRepo.AddAsync(new User { Id = 4, Username = "userito", Password = "passwordini", Roles = [new() { RoleName = "learner" }] }).Wait();

                // Add fake course data
                courseRepo.AddAsync(new CreateCourseDto { Title = "Intro to Testing", Language = "ENG" }).Wait(); // ID = 1
                courseRepo.AddAsync(new CreateCourseDto { Title = "Advanced Testing", Language = "ENG" }).Wait(); // ID = 2
                learningStepRepo.AddAsync(new LearningStep { CourseId = 1, StepOrder = 1, Type = "Text", Content = "Welcome to Intro to Testing!" }).Wait();
                learningStepRepo.AddAsync(new LearningStep { CourseId = 1, StepOrder = 2, Type = "QuestionMC", Content = "What is unit testing?|idk|something else" }).Wait();
                learningStepRepo.AddAsync(new LearningStep { CourseId = 1, StepOrder = 3, Type = "QuestionMC", Content = "What is unit testing?|idk|something else" }).Wait();

                // 3. Add the InMemory instances to the DI container

                services.AddSingleton<ICourseRepository>(courseRepo);
                services.AddSingleton<IRepositoryID<Course, CreateCourseDto, Course, int>>(courseRepo);
                services.AddSingleton<IRepositoryID<User, User, User, int>>(userRepo);
                services.AddSingleton<ICourseProgressRepository>(courseRepo);
                services.AddSingleton<IRepositoryID<LearningStep, LearningStep, LearningStep, (int, int)>>(learningStepRepo);

                // Override Auth Service
                services.AddSingleton<IAuthService, AuthService>();
            });

            // To remove the nasty console bullshit during testing
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });
        });
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Dummy_CanReachServer()
    {
        var response = await _client.GetAsync("/status");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Dummy_FullAuthLifecycle()
    {
        await PureTests.AuthLifecycle(_client, _testOutputHelper);
    }

    [Fact]
    public async Task Dummy_FullCourseLifeCycle()
    {
        await PureTests.CourseLifeCycle(_client, GenerateJwtToken, _testOutputHelper);
    }

    [Fact]
    public async Task Dummy_FullLearningStepsLifeCycle()
    {
        await PureTests.LearningStepsLifeCycle(_client, GenerateJwtToken, _testOutputHelper);
    }

    [Fact]
    public async Task Dummy_FullCourseProgressLifeCycle()
    {
        await PureTests.CourseProgressLifeCycle(_client, GenerateJwtToken, _testOutputHelper);
    }

    [Fact]
    public async Task Dummy_ForbiddenCourseProgressAccess()
    {
        await PureTests.CourseProgressAuth(_client, GenerateJwtToken, _testOutputHelper);
    }

    [Fact]
    public async Task Dummy_CheckCourseCreation()
    {
        await PureTests.CheckCourseCreation(_client, GenerateJwtToken, _testOutputHelper);
    }

    [Fact]
    public async Task Dummy_GettingIdWorks()
    {
        await PureTests.GettingIdFromTokenWorks(_client, GenerateJwtToken, _testOutputHelper);
    }
}