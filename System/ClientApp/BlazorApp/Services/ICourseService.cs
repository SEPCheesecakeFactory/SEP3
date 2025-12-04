using System;
using BlazorApp.Entities;

namespace BlazorApp.Services;

public interface ICourseService
{
    public Task<List<Entities.Course>> GetCourses();

    // progress
    Task<int> GetCourseProgressAsync(int userId, int courseId);
    Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep);
    Task<List<LeaderboardEntry>> GetLeaderboardAsync();
}
