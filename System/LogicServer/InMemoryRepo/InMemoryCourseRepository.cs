using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryCourseRepository(IRepositoryID<LearningStep, LearningStep, LearningStep, (int, int)> learningStepRepository) : ICourseRepository, ICourseProgressRepository
{
    private readonly List<Course> courses = [];
    private readonly Dictionary<(int userId, int courseId), int> progress = [];
    private readonly IRepositoryID<LearningStep, LearningStep, LearningStep, (int, int)> _learningStepRepository = learningStepRepository;
    public Task<Course> AddAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Id = courses.Count != 0 ? courses.Max(c => c.Id) + 1 : 1,
            Title = dto.Title ?? "",
            Description = dto.Description ?? "",
            Language = dto.Language ?? "",
            Category = dto.Category ?? "",
            TotalSteps = 0,
            AuthorId = dto.AuthorId,
            ApprovedBy = null
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
    public Task DeleteAsync(int courseId, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Course> GetSingleAsync(int id)
    {
        var course = courses.SingleOrDefault(c => c.Id == id)
            ?? throw new NotFoundException($"Course with ID '{id}' not found");
        return Task.FromResult(ProcessedCourse(course));
    }

    public IQueryable<Course> GetMany()
    {
        return courses.Select(ProcessedCourse).AsQueryable();
    }

    public Task ClearAsync()
    {
        courses.Clear();
        return Task.CompletedTask;
    }

    public Task<int> GetCourseProgressAsync(int userId, int courseId)
    {
        var key = (userId, courseId);
        return Task.FromResult(progress.GetValueOrDefault(key, 1));
    }

    public Task<int> UpdateCourseProgressAsync(int userId, int courseId, int currentStep)
    {
        var key = (userId, courseId);
        progress[key] = currentStep;
        return Task.FromResult(currentStep);
    }

    public IQueryable<Course> GetManyByUserId(int? userId = null)
    {
        return courses.AsQueryable();
    }

    private Course ProcessedCourse(Course course)
    {
        return new Course
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Language = course.Language,
            Category = course.Category,
            TotalSteps = _learningStepRepository.GetMany().Count(ls => ls.CourseId == course.Id),
            AuthorId = course.AuthorId,
            ApprovedBy = course.ApprovedBy
        };
    }
}