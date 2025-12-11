using System;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestsProject;

public class AuthTesting : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AuthTesting(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Login_With_Valid_Credentials_Returns_Jwt_Token()
    {
        var token = await TestingUtils.GetTokenWithRoles(["admin"],_client);
        // Assert
        token.Should().NotBeNullOrEmpty();
    }
}
