using System;
using System.Text;
using System.Text.Json;
using BlazorApp.Entities;
using System.Net.Http.Json;
using BlazorApp.Shared;  

namespace BlazorApp.Services;

public class HttpCourseService : ICourseService
{
    private readonly HttpClient client;

    public HttpCourseService(HttpClient client)
    {
        this.client = client;
    }

    
    // GET COURSES
    public async Task<Optional<List<Course>>> GetCourses()
    {
        try
        {
            
            // var result = await client.GetFromJsonAsync<List<Course>>("courses");
            // return new List<Course>(result ?? new List<Course>());

            var result = await client.GetFromJsonAsync<List<Course>>("courses");
            return Optional<List<Course>>.Success(result ?? new List<Course>());
        }
        catch (Exception ex)
        {
            return Optional<List<Course>>.Error("Failed to load courses: " + ex.Message);
        }
    }

    public async Task<Optional<List<Course>>> GetCourses(int? userId = null)
    {
        var uri = userId.HasValue ? $"courses/my-courses/{userId}" : "courses";

        try
        {
            // var result = await client.GetFromJsonAsync<List<Course>>(uri);
            // return result ?? new List<Course>();

            var result = await client.GetFromJsonAsync<List<Course>>(uri);
            return Optional<List<Course>>.Success(result ?? new List<Course>());
        }
        catch (Exception ex)
        {
            return Optional<List<Course>>.Error("Failed to load courses: " + ex.Message);
        }
    }

    // CREATE DRAFT
    public async Task<Optional<bool>> CreateDraft(CreateDraftDto dto)
    {
        try
        {
            // var response = await client.PostAsJsonAsync("drafts", dto);
            // if (!response.IsSuccessStatusCode)
            // {
            //     throw new Exception($"Error creating draft: {response.ReasonPhrase}");
            // }

            var response = await client.PostAsJsonAsync("drafts", dto);
            if (!response.IsSuccessStatusCode)
                return Optional<bool>.Error("Error creating draft");

            return Optional<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Optional<bool>.Error(ex.Message);
        }
    }

    // GET DRAFTS
 public async Task<Optional<List<Draft>>> GetDrafts()
{
    try
    {
        var response = await client.GetAsync("drafts");

        if (!response.IsSuccessStatusCode)
        {
            var serverMsg = await response.Content.ReadAsStringAsync();
            return Optional<List<Draft>>.Error(
                string.IsNullOrWhiteSpace(serverMsg)
                    ? $"Server error: {response.StatusCode}"
                    : serverMsg
            );
        }

        var result = await response.Content.ReadFromJsonAsync<List<Draft>>();
        return Optional<List<Draft>>.Success(result ?? new List<Draft>());
    }
    catch (HttpRequestException)
    {
        return Optional<List<Draft>>.Error("NO_CONNECTION");
    }
    catch (Exception ex)
    {
        return Optional<List<Draft>>.Error($"Unexpected error: {ex.Message}");
    }
}



    // APPROVE DRAFT
    public async Task<Optional<bool>> ApproveDraft(int draftId, int adminId)
{
    try
    {
        var response = await client.PutAsJsonAsync($"drafts/{draftId}", adminId);

        if (!response.IsSuccessStatusCode)
        {
            var serverMessage = await response.Content.ReadAsStringAsync();

            var errorMessage = string.IsNullOrWhiteSpace(serverMessage)
                ? $"Server error: {response.StatusCode}"
                : serverMessage;

            return Optional<bool>.Error(errorMessage);
        }

        return Optional<bool>.Success(true);
    }
    catch (HttpRequestException)
    {
        return Optional<bool>.Error("NO_CONNECTION");
    }
    catch (Exception ex)
    {
        return Optional<bool>.Error("Unexpected error: " + ex.Message);
    }
}


    // COURSE PROGRESS
    public async Task<Optional<int>> GetCourseProgressAsync(int userId, int courseId)
    {
        try
        {
            /*
            var response = await client.GetAsync($"CourseProgress/{userId}/{courseId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            return 1;
            */

            var response = await client.GetAsync($"CourseProgress/{userId}/{courseId}");
            if (!response.IsSuccessStatusCode)
                return Optional<int>.Success(1);

            var value = await response.Content.ReadFromJsonAsync<int>();

            return Optional<int>.Success(value);
        }
        catch (Exception ex)
        {
            return Optional<int>.Error("Error loading course progress: " + ex.Message);
        }
    }

    public async Task<Optional<bool>> UpdateCourseProgressAsync(int userId, int courseId, int currentStep)
    {
        var dto = new { UserId = userId, CourseId = courseId, CurrentStep = currentStep };

        try
        {
            // await client.PostAsJsonAsync("CourseProgress", dto);

            var response = await client.PostAsJsonAsync("CourseProgress", dto);
            if (!response.IsSuccessStatusCode)
                return Optional<bool>.Error("Failed to update course progress");

            return Optional<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Optional<bool>.Error(ex.Message);
        }
    }

    // UPDATE COURSE
    public async Task<Optional<bool>> UpdateCourse(int id, Course course)
    {
        try
        {
            /*
            var json = JsonSerializer.Serialize(course);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"courses/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            */

            var response = await client.PutAsJsonAsync($"courses/{id}", course);
            if (!response.IsSuccessStatusCode)
                return Optional<bool>.Error("Failed to update course");

            return Optional<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Optional<bool>.Error(ex.Message);
        }
    }

    // LEADERBOARD
    public async Task<Optional<List<LeaderboardEntry>>> GetLeaderboardAsync()
    {
        try
        {
            // var result = await client.GetFromJsonAsync<List<LeaderboardEntry>>("Leaderboard");
            // return result ?? new List<LeaderboardEntry>();

            var result = await client.GetFromJsonAsync<List<LeaderboardEntry>>("Leaderboard");
            return Optional<List<LeaderboardEntry>>.Success(result ?? new List<LeaderboardEntry>());
        }
        catch (Exception ex)
        {
            return Optional<List<LeaderboardEntry>>.Error("Failed to load leaderboard: " + ex.Message);
        }
    }
    public async Task CreateCategory(CreateCourseCategoryDto dto)
    {
        var response = await client.PostAsJsonAsync("categories", dto);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error creating category: {response.ReasonPhrase}");
        }
    }
    public async Task<List<CourseCategory>> GetCategories()
    {
        var result = await client.GetFromJsonAsync<List<CourseCategory>>("categories");
        return result ?? new List<CourseCategory>();
    }
}
