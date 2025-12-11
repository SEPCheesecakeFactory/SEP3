using System;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace TestsProject;

public static class PureTests
{
    public static async Task AuthLifecycle(HttpClient client)
    {
        // 1. REGISTER
        var registerRequest = new
        {
            Username = "testuser",
            Password = "testpassword",
            PasswordRepeat = "testpassword",
            Roles = new [] { new { RoleName = "learner" } }
        };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequest);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 2. LOGIN
        var loginRequest = new
        {
            Username = "testuser",
            Password = "testpassword"
        };

        var loginResponse = await client.PostAsJsonAsync("/Auth/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await loginResponse.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrEmpty();

        // 3. ACCESS PROTECTED RESOURCE
        client.Login(token);

        // TODO: test if the token allows access to a protected resource
    }
    public static async Task CourseLifeCycle(HttpClient client, Func<IEnumerable<string>, string> TokenProvider)
    {
        // Test GET /courses (requires auth)
        var token = TokenProvider(["learner"]);
        client.Login(token);

        var response = await client.GetAsync("/courses");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test POST /drafts (create course)
        token = TokenProvider(["teacher"]);
        client.Login(token);

        var createDto = new
        {
            Language = "ENG",
            Title = "Test Course",
            Description = "Test Description",
            Category = "History",
            AuthorId = 1
        };
        var createResponse = await client.PostAsJsonAsync("/drafts", createDto);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCourse = await createResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        createdCourse.Should().NotBeNull();
        createdCourse!["Title"].Should().Be("Test Course");

        // Test GET /courses/my-courses/{userId}
        var myCoursesResponse = await client.GetAsync("/courses/my-courses/1");
        myCoursesResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test PUT /courses/{id} (update)
        createdCourse["Description"] = "Updated Description";
        var updateResponse = await client.PutAsJsonAsync($"/courses/{createdCourse["Id"]}", createdCourse);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test PUT /drafts/{id} (approve draft) - requires admin
        var adminToken = TokenProvider(["admin"]);
        client.Login(adminToken);
        var approveResponse = await client.PutAsJsonAsync($"/drafts/{createdCourse["Id"]}", 1); // approvedBy = 1
        approveResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
