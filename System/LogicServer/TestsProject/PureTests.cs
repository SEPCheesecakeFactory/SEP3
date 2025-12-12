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
        var uniqueSuffix = TestingUtils.GetCurrentRandomSuffix();
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
    /// <summary>
    /// Tries to go through full course progress lifecycle:
    /// Make a new user with no progress anywhere
    /// Get progress for course 1 (should be 1)
    /// Update progress for course 1 to step 3
    /// Get progress for course 1 (should be 3)
    /// Update progress for course 1 to step 5
    /// Get progress for course 1 (should be 5)
    /// </summary>
    /// <param name="client"></param>
    /// <param name="TokenProvider"></param>
    /// <param name="testOutputHelper"></param>
    /// <returns></returns>
    public static async Task CourseProgressLifeCycle(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
    {
        // Register new user
        var uniqueSuffix = TestingUtils.GetCurrentRandomSuffix();
        var uniqueUsername = "testuser" + uniqueSuffix;

        var registerRequest = new
        {
            Username = uniqueUsername,
            Password = "passwordini",
            PasswordRepeat = "passwordini",
            Roles = new[] { new { RoleName = "learner" } }
        };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequest);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Login as the new user
        var loginRequest = new
        {
            Username = uniqueUsername,
            Password = "passwordini"
        };

        var loginResponse = await client.PostAsJsonAsync("/Auth/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await loginResponse.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrEmpty();

        client.Login(token);

        // Get user ID from token
        var userId = TestingUtils.GetUserIdFromToken(token);

        // Get progress for course 1 (should be 0)
        var getResponse1 = await client.GetAsync($"/courseprogress/{userId}/1");
        getResponse1.StatusCode.Should().Be(HttpStatusCode.OK);
        var progress1 = int.Parse(await getResponse1.Content.ReadAsStringAsync());
        progress1.Should().Be(1);

        // Update progress for course 1 to step 3
        var updateDto1 = new
        {
            UserId = userId,
            CourseId = 1,
            CurrentStep = 3
        };
        var postResponse1 = await client.PostAsJsonAsync("/courseprogress", updateDto1);
        postResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

        // Get progress for course 1 (should be 3)
        var getResponse2 = await client.GetAsync($"/courseprogress/{userId}/1");
        getResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
        var progress2 = int.Parse(await getResponse2.Content.ReadAsStringAsync());
        progress2.Should().Be(3);

        // Update progress for course 1 to step 5
        var updateDto2 = new
        {
            UserId = userId,
            CourseId = 1,
            CurrentStep = 5
        };
        var postResponse2 = await client.PostAsJsonAsync("/courseprogress", updateDto2);
        postResponse2.StatusCode.Should().Be(HttpStatusCode.OK);

        // Get progress for course 1 (should be 5)
        var getResponse3 = await client.GetAsync($"/courseprogress/{userId}/1");
        getResponse3.StatusCode.Should().Be(HttpStatusCode.OK);
        var progress3 = int.Parse(await getResponse3.Content.ReadAsStringAsync());
        progress3.Should().Be(5);
    }
    /// <summary>
    /// Register as new user
    /// Try to change progress of user 1 on course 1
    /// If succeeded, test fails
    /// </summary>
    /// <returns></returns>
    public static async Task CourseProgressAuth(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
    {
        // Register new user
        var uniqueSuffix = TestingUtils.GetCurrentRandomSuffix();
        var uniqueUsername = "testuser" + uniqueSuffix;

        var registerRequest = new
        {
            Username = uniqueUsername,
            Password = "passwordini",
            PasswordRepeat = "passwordini",
            Roles = new[] { new { RoleName = "learner" } }
        };

        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequest);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Login as the new user
        var loginRequest = new
        {
            Username = uniqueUsername,
            Password = "passwordini"
        };

        var loginResponse = await client.PostAsJsonAsync("/Auth/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await loginResponse.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrEmpty();

        client.Login(token);

        // Try to change progress of user 1 on course 1
        var updateDto = new
        {
            UserId = 1,
            CourseId = 1,
            CurrentStep = 5
        };
        var postResponse = await client.PostAsJsonAsync("/courseprogress", updateDto);
        testOutputHelper?.WriteLine(await postResponse.Content.ReadAsStringAsync());
        postResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    /// <summary>
    /// Creating a course requires for one learning step to be present after creation. 
    /// This test checks that creating a course works as expected.
    /// The Learning Step should only appear after approval of the course by the admin.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="TokenProvider"></param>
    /// <param name="testOutputHelper"></param>
    /// <returns></returns>
    public static async Task CheckCourseCreation(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
    {
        // Create course as teacher
        var token = TokenProvider(["teacher"]);
        client.Login(token);
        var id = TestingUtils.GetUserIdFromToken(token);

        var createDto = new
        {
            Language = "ENG",
            Title = "Test Course for Creation Check",
            Description = "Test Description",
            Category = "Science",
            AuthorId = id
        };

        var createResponse = await client.PostAsJsonAsync("/drafts", createDto);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdCourse = await createResponse.Content.ReadFromJsonAsync<JsonObject>();
        createdCourse.Should().NotBeNull();
        var courseId = createdCourse!["id"]!.GetValue<int>();

        // Check that no learning steps exist for the course yet
        var getFirstStep = await client.GetAsync($"/learningsteps/{courseId}_1");
        getFirstStep.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Approve course as admin
        var adminToken = TokenProvider(["admin"]);
        id = TestingUtils.GetUserIdFromToken(adminToken);
        client.Login(adminToken);
        var approveResponse = await client.PutAsJsonAsync($"/drafts/{courseId}", id); // approvedBy = current user id
        approveResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Check that the first learning step now exists
        var getStepsResponseAfter = await client.GetAsync($"/learningsteps/{courseId}_1");
        getStepsResponseAfter.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>
    /// Should not allow creating a draft under someone else's user ID
    /// </summary>
    /// <param name="client"></param>
    /// <param name="TokenProvider"></param>
    /// <param name="testOutputHelper"></param>
    /// <returns></returns>
    public static async Task CreateDraftAsSomeoneElse(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Should not allow approving a draft under someone else's user ID
    /// </summary>
    /// <param name="client"></param>
    /// <param name="TokenProvider"></param>
    /// <param name="testOutputHelper"></param>
    /// <returns></returns>
    public static async Task ApproveDraftAsSomeoneElse(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Logs in and tries to get a user ID from the token
    /// </summary>
    public static async Task GettingIdFromTokenWorks(HttpClient client, Func<IEnumerable<string>, string> TokenProvider, ITestOutputHelper? testOutputHelper = null)
    {
        // Register new user
        var uniqueSuffix = TestingUtils.GetCurrentRandomSuffix();
        var uniqueUsername = "testuser" + uniqueSuffix;
        var registerRequest = new
        {
            Username = uniqueUsername,
            Password = "passwordini",
            PasswordRepeat = "passwordini",
            Roles = new[] { new { RoleName = "learner" } }
        };
        var registerResponse = await client.PostAsJsonAsync("/Auth/register", registerRequest);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Login as the new user
        var loginRequest = new
        {
            Username = uniqueUsername,
            Password = "passwordini"
        };
        var loginResponse = await client.PostAsJsonAsync("/Auth/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var token = await loginResponse.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrEmpty();
        var userId = TestingUtils.GetUserIdFromToken(token);
        testOutputHelper?.WriteLine($"Extracted user ID from token: {userId}");
        Assert.True(userId > 0, "User ID extracted from token should be greater than 0");
    }
}
