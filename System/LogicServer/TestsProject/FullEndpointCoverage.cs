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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;
using InMemoryRepositories;

namespace TestsProject;

public class FullEndpointCoverage : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public FullEndpointCoverage(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Can_Reach_Server()
    {
        var response = await _client.GetAsync("/status");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Full_User_Registration_And_Login_Cycle()
    {
        // 1. REGISTER
        var registerRequest = new RegisterRequest
        {
            Username = "testuser",
            Password = "testpassword",
            PasswordRepeat = "testpassword",
            Roles = [new Role { RoleName = "learner" }]
        };

        var registerResponse = await _client.PostAsJsonAsync("/Auth/register", registerRequest);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 2. LOGIN
        var loginRequest = new LoginRequest
        {
            Username = "testuser",
            Password = "testpassword"
        };

        var loginResponse = await _client.PostAsJsonAsync("/Auth/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await loginResponse.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrEmpty();

        // 3. ACCESS PROTECTED RESOURCE
        _client.Login(token);

        // TODO: test if the token allows access to a protected resource
    }

    [Fact]
    public async Task Full_Course_Lifecycle_Real()
    {
        var client = _factory.CreateClient();

        // Test GET /courses (requires auth)
        var token = await TestingUtils.GetTokenWithRoles(["learner"], client);
        client.Login(token);

        var response = await client.GetAsync("/courses");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test POST /drafts (create course)
        token = await TestingUtils.GetTokenWithRoles(["teacher"], client);
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
        var adminToken = await TestingUtils.GetTokenWithRoles(["admin"], client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var approveResponse = await client.PutAsJsonAsync($"/drafts/{createdCourse.Id}", 1); // approvedBy = 1
        approveResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
