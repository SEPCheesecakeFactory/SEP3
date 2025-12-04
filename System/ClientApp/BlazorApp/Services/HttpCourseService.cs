using System;
using System.Text;
using System.Text.Json;
using BlazorApp.Entities;
using System.Net.Http.Json;
namespace BlazorApp.Services;

using BlazorApp.Entities;

public class HttpCourseService : ICourseService
{
    private readonly HttpClient client;
    public HttpCourseService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<List<Course>> GetCourses()
    {
        var result = await client.GetFromJsonAsync<List<Course>>("courses");
        return new List<Course>(result ?? new List<Course>());
    }

    public async Task CreateDraft(CreateDraftDto dto)
    {
        var response = await client.PostAsJsonAsync("drafts", dto);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error creating draft: {response.ReasonPhrase}");
        }
    }

    public async Task<List<Draft>> GetDrafts()
    {
        var result = await client.GetFromJsonAsync<List<Draft>>("drafts");
        return result ?? new List<Draft>();
    }

    public async Task ApproveDraft(int draftId, int adminId)
    {
        var response = await client.PutAsJsonAsync($"drafts/{draftId}", adminId);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error approving draft: {response.ReasonPhrase}");
        }
    }
    public async Task<int> GetCourseProgressAsync(int userId, int courseId)
    {
        try
        {
            var response = await client.GetAsync($"CourseProgress/{userId}/{courseId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            return 1;
        }
        catch
        {
            return 1;
        }
    }

    public async Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep)
    {
        var dto = new { UserId = userId, CourseId = courseId, CurrentStep = currentStep };

        await client.PostAsJsonAsync("CourseProgress", dto);
    }
    public async Task UpdateCourse(int id, Course course)
    {
        var json = JsonSerializer.Serialize(course);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"courses/{id}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

    public async Task<List<LeaderboardEntry>> GetLeaderboardAsync()
    {
        var result = await client.GetFromJsonAsync<List<LeaderboardEntry>>("Leaderboard");
        return result ?? new List<LeaderboardEntry>();
    }
}
