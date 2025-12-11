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
    public async Task Full_Course_Lifecycle_Real()
    {
        // 1. AUTHENTICATE (Teacher)
        var teacherToken = await TestingUtils.GetTokenWithRoles(["teacher"], _client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", teacherToken);

        // 2. CREATE
        var createDto = new CreateCourseDto
        {
            Title = $"Test Course {Guid.NewGuid()}",
            Description = "Testing the full system",
            Language = "English",
            Category = "History"
        };

        Course? createdCourse = null;

        try
        {
            var createRes = await _client.PostAsJsonAsync("/drafts", createDto);
            createRes.StatusCode.Should().Be(HttpStatusCode.Created);

            createdCourse = await createRes.Content.ReadFromJsonAsync<Course>();
            createdCourse.Should().NotBeNull();
            createdCourse!.Id.Should().BeGreaterThan(0);

            // 3. READ
            var getRes = await _client.GetAsync($"/courses/{createdCourse.Id}");

            // 4. APPROVE (Admin Workflow)
            var adminToken = await TestingUtils.GetTokenWithRoles(["admin"], _client);

            var adminClient = _factory.CreateClient();
            adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var approveRes = await adminClient.PutAsJsonAsync($"/drafts/{createdCourse.Id}", 1);
            approveRes.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            // 5. CLEANUP
            if (createdCourse != null)
            {
                await _client.DeleteAsync($"/courses/{createdCourse.Id}");
            }
        }
    }
}
