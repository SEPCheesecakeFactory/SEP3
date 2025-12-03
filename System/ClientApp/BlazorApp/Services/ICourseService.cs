using System;
using BlazorApp.Entities;
namespace BlazorApp.Services;

public interface ICourseService
{
    public Task<List<Entities.Course>> GetCourses();
    Task UpdateCourse(int id, Course course);
}
