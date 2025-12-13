using System;
using BlazorApp.Entities;
using BlazorApp.Shared;

namespace BlazorApp.Services;

public interface ICourseService
{
    // UPDATE COURSE
    Task<Optional<Course>> UpdateCourse(int id, Course course);

    // GET COURSES
    Task<Optional<List<Course>>> GetCourses();
    Task<Optional<List<Course>>> GetCourses(int? userId = null);

    // CREATE DRAFT
    Task<Optional<Draft>> CreateDraft(CreateDraftDto dto);

    // GET DRAFTS
    Task<Optional<List<Draft>>> GetDrafts();

    // APPROVE DRAFT
    Task<Optional<Draft>> ApproveDraft(int draftId, int adminId);

    // PROGRESS
    Task<Optional<int>> GetCourseProgressAsync(int userId, int courseId);
    Task<Optional<int>> UpdateCourseProgressAsync(int userId, int courseId, int currentStep);
    Task DeleteCourseProgressAsync(int courseId, int userId);


    // LEADERBOARD
    Task<Optional<List<LeaderboardEntry>>> GetLeaderboardAsync();
    Task<Optional<CourseCategory>> CreateCategory(CreateCourseCategoryDto createCourseCategoryDto);
    Task<List<CourseCategory>> GetCategories();

    // LANGUAGES
    Task<Optional<Language>> CreateLanguage(CreateLanguageDto dto);
    Task<List<Language>> GetLanguages();
}
