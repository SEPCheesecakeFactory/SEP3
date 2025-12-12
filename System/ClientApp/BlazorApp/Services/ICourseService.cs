using System;
using BlazorApp.Entities;

namespace BlazorApp.Services;

public interface ICourseService
{
    Task UpdateCourse(int id, Course course);
    public Task<List<Course>> GetCourses();
    public Task<List<Course>> GetCourses(int? userId = null);
    public Task CreateDraft(CreateDraftDto dto);
    public Task<List<Draft>> GetDrafts();
    public Task ApproveDraft(int draftId, int adminId);
    public Task DisapproveDraft(int draftId, int adminId);

    Task CreateLanguage(CreateLanguageDto dto);
    Task<List<Language>> GetLanguages();


    

    // progress
    Task<int> GetCourseProgressAsync(int userId, int courseId);
    Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep);
    Task<List<LeaderboardEntry>> GetLeaderboardAsync();
    // categories
    public Task CreateCategory(CreateCourseCategoryDto createCourseCategoryDto);
    public Task<List<CourseCategory>> GetCategories(); 
}
