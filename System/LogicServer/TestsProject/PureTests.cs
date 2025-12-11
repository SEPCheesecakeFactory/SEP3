using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Xunit.Abstractions;

namespace TestsProject;

public static class PureTests
{
    public static async Task AuthLifecycle(HttpClient client, ITestOutputHelper? testOutputHelper = null)
    {
        // Ensure unique usernames by appending a timestamp + random number
        var uniqueSuffix = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{new Random().Next(1000, 9999)}";
        var uniqueUsername = "testuser" + uniqueSuffix;
        // 1. REGISTER
        var registerRequest = new
        {
            Username = uniqueUsername,
            Password = "passwordini",
            PasswordRepeat = "passwordini",
            Roles = new[] { new { RoleName = "learner" } }
        };

        testOutputHelper?.WriteLine($"Registering user with username: {uniqueUsername} and password passwordini");

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequest);
        testOutputHelper?.WriteLine(await registerResponse.Content.ReadAsStringAsync());
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // 2. LOGIN
        var loginRequest = new
        {
            Username = uniqueUsername,
            Password = "passwordini"
        };

        var loginResponse = await client.PostAsJsonAsync("/Auth/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await loginResponse.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrEmpty();

        // 3. ACCESS PROTECTED RESOURCE
        client.Login(token);

        // TODO: test if the token allows access to a protected resource
    }
    public static async Task CourseLifeCycle(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
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

        var createdCourse = await createResponse.Content.ReadFromJsonAsync<JsonObject>();
        createdCourse.Should().NotBeNull();
        testOutputHelper?.WriteLine(await createResponse.Content.ReadAsStringAsync());
        createdCourse!["title"]!.GetValue<string>().Should().Be("Test Course");

        // Test GET /courses/my-courses/{userId}
        var myCoursesResponse = await client.GetAsync("/courses/my-courses/1");
        myCoursesResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test PUT /courses/{id} (update)
        createdCourse["description"] = "Updated Description";
        var updateResponse = await client.PutAsJsonAsync($"/courses/{createdCourse["id"]}", createdCourse);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test PUT /drafts/{id} (approve draft) - requires admin
        var adminToken = TokenProvider(["admin"]);
        client.Login(adminToken);
        var approveResponse = await client.PutAsJsonAsync($"/drafts/{createdCourse["id"]}", 1); // approvedBy = 1
        approveResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    public static async Task LearningStepsLifeCycle(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
    {
        // Test GET /learningsteps/1_1 (requires auth)
        testOutputHelper?.WriteLine("Retrieving learning step with ID: 1_1");
        var token = TokenProvider(["learner"]);
        client.Login(token);

        var response = await client.GetAsync("/learningsteps/1_1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Find out what is the highest step at course 1 and make stepOrder to be +1
        // TODO: Add the ability to retrieve single
        
        var courseForTesting = 1;

        testOutputHelper?.WriteLine("Retrieving all courses to determine current step count for course " + courseForTesting);

        var allCourses = await client.GetAsync("/courses");
        allCourses.StatusCode.Should().Be(HttpStatusCode.OK);

        testOutputHelper?.WriteLine("All courses: " + await allCourses.Content.ReadAsStringAsync());

        /*var courseResponse = await client.GetAsync($"/courses/{courseForTesting}");
        courseResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var theCourse = courseResponse.Content.ReadFromJsonAsync<JsonObject>();
        var stepsInTheCourse = theCourse.Result!["totalsteps"]!.GetValue<int>();*/

        var allCoursesObject = await allCourses.Content.ReadFromJsonAsync<JsonArray>();
        var theCourse = allCoursesObject!.First(c => c!["id"]!.GetValue<int>() == courseForTesting);
        var stepsInTheCourse = theCourse!["totalsteps"]!.GetValue<int>();

        testOutputHelper?.WriteLine($"Course {courseForTesting} has {stepsInTheCourse} steps. Next step order will be {stepsInTheCourse + 1}");

        var stepOrder = stepsInTheCourse + 1;

        // Test POST /learningsteps (create learning step)
        token = TokenProvider(["teacher"]);
        client.Login(token);        

        var createDto = new
        {
            CourseId = courseForTesting,
            StepOrder = stepOrder,
            Type = "Text",
            Content = "Sample Content"
        };

        testOutputHelper?.WriteLine($"Creating learning step for course {courseForTesting} with step order {stepOrder}");

        var createResponse = await client.PostAsJsonAsync("/learningsteps", createDto);
        testOutputHelper?.WriteLine(await createResponse.Content.ReadAsStringAsync());
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdStep = await createResponse.Content.ReadFromJsonAsync<JsonObject>();
        createdStep.Should().NotBeNull();
        testOutputHelper?.WriteLine(await createResponse.Content.ReadAsStringAsync());
        createdStep!["courseId"]!.GetValue<int>().Should().Be(courseForTesting);
        createdStep!["stepOrder"]!.GetValue<int>().Should().Be(stepOrder);

        // Test GET /learningsteps/{id}
        testOutputHelper?.WriteLine($"Retrieving learning step with ID: {courseForTesting}_{stepOrder}");
        var getSingleResponse = await client.GetAsync($"/learningsteps/{courseForTesting}_{stepOrder}");
        getSingleResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test PUT /learningsteps/{id} (update)
        var updatedStep = new
        {
            CourseId = courseForTesting,
            StepOrder = stepOrder,
            Type = "QuestionMC",
            Content = "Updated Content"
        };
        testOutputHelper?.WriteLine($"Updating learning step with ID: {courseForTesting}_{stepOrder}");
        var updateResponse = await client.PutAsJsonAsync($"/learningsteps/{courseForTesting}_{stepOrder}", updatedStep);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test DELETE /learningsteps/{id}
        /*testOutputHelper?.WriteLine($"Deleting learning step with ID: {courseForTesting}_{stepOrder}");
        var deleteResponse = await client.DeleteAsync($"/learningsteps/{courseForTesting}_{stepOrder}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);*/
        // TODO: Handle clean-up later
    }
    public static async Task CourseProgressLifeCycle(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
    {
        // TODO: should only be able to post own progress unless admin/teacher
        var token = TokenProvider(["learner"]);
        client.Login(token);

        // Test POST /courseprogress (update progress)
        var updateDto = new
        {
            UserId = 1,
            CourseId = 1,
            CurrentStep = 2
        };
        var postResponse = await client.PostAsJsonAsync("/courseprogress", updateDto);
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Test GET /courseprogress/{userId}/{courseId}
        var getResponse2 = await client.GetAsync("/courseprogress/1/1");
        getResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
        var progress2 = await getResponse2.Content.ReadFromJsonAsync<int>();
        progress2.Should().Be(2);

        // Test POST /courseprogress (update progress)
        var updateDto2 = new
        {
            UserId = 1,
            CourseId = 1,
            CurrentStep = 4
        };
        var postResponse2 = await client.PostAsJsonAsync("/courseprogress", updateDto2);
        postResponse2.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify update
        var getResponse3 = await client.GetAsync("/courseprogress/1/1");
        getResponse3.StatusCode.Should().Be(HttpStatusCode.OK);
        var progress3 = await getResponse3.Content.ReadFromJsonAsync<int>();
        progress3.Should().Be(4);
    }
}
