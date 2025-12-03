using System;
using System.Text;
using System.Text.Json;
using BlazorApp.Entities;
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
}
