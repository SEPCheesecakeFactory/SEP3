using System;
using BlazorApp.Entities;
using System.Net.Http.Json;

using System.Net.Http.Json;
namespace BlazorApp.Services;

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
}
