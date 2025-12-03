using System;
using BlazorApp.Entities;
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
}
