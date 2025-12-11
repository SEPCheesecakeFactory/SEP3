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
using Xunit.Abstractions;

namespace TestsProject;

public class FullEndpointCoverage : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private static Func<IEnumerable<string>, string> TokenProvider(HttpClient _thisClient) => roles => TestingUtils.GetTokenWithRoles(roles, _thisClient).Result;
    private readonly ITestOutputHelper _testOutputHelper;

    public FullEndpointCoverage(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Can_Reach_Server()
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
        await PureTests.CourseLifeCycle(_client, TokenProvider(_client), _testOutputHelper);
    }
}
