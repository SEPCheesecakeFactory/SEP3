using System;

namespace BlazorApp.Services;

public interface ICourseService
{
    public Task<List<Entities.Course>> GetCourses();
}
