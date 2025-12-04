using System;
using System.Net.Http.Json;
namespace BlazorApp.Services;

public class HttpCourseService : ICourseService
{
    private readonly HttpClient client;
    public HttpCourseService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<List<Entities.Course>> GetCourses()
    {
        var result = await client.GetFromJsonAsync<List<Entities.Course>>("courses");
        return new List<Entities.Course>(result ?? new List<Entities.Course>());
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
