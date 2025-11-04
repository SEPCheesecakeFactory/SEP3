using System;

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
}
