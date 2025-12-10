using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryCourseRepository : ICourseRepository
{
    private readonly List<Course> courses = [];

    public Task<Course> AddAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Id = courses.Count != 0 ? courses.Max(c => c.Id) + 1 : 1,
            Title = dto.Title ?? "",
            Description = dto.Description ?? "",
            Category = dto.Category ?? "",
            Language = dto.Language ?? ""
        };
        courses.Add(course);
        return Task.FromResult(course);
    }

    public Task<Course> UpdateAsync(Course course)
    {
        var existing = courses.SingleOrDefault(c => c.Id == course.Id)
            ?? throw new NotFoundException($"Course with ID '{course.Id}' not found");
        courses.Remove(existing);
        courses.Add(course);
        return Task.FromResult(course);
    }

    public Task DeleteAsync(int id)
    {
        var course = courses.SingleOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"Course with ID '{id}' not found");
        courses.Remove(course);
        return Task.CompletedTask;
    }

    public Task<Course> GetSingleAsync(int id)
    {
        var course = courses.SingleOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"Course with ID '{id}' not found");
        return Task.FromResult(course);
    }

    public IQueryable<Course> GetMany()
    {
        return courses.AsQueryable();
    }

    public Task ClearAsync()
    {
        courses.Clear();
        return Task.CompletedTask;
    }

    public Task<int> GetCourseProgressAsync(int userId, int courseId)
    {
        // For simplicity, return 1
        return Task.FromResult(1);
    }

    public Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep)
    {
        // Do nothing
        return Task.CompletedTask;
    }

    public IQueryable<Course> GetManyByUserId(int? userId = null)
    {
        return courses.AsQueryable();
    }
}