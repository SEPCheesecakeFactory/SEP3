using System;
using BlazorApp.Entities;

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
}
