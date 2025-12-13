using System;
using System.Text;
using System.Text.Json;
using BlazorApp.Entities;
using System.Net.Http.Json;
using BlazorApp.Shared;

namespace BlazorApp.Services;

public class HttpCourseService(HttpCrudService httpCrudService) : ICourseService
{
    // GET COURSES
    public async Task<Optional<List<Course>>> GetCourses() => await httpCrudService.GetAsync<List<Course>>("courses");
    public async Task<Optional<List<Course>>> GetCourses(int? userId = null)
    {
        var uri = userId.HasValue ? $"courses/my-courses/{userId}" : "courses";

        return await httpCrudService.GetAsync<List<Course>>(uri);
    }

    // CREATE DRAFT
    public async Task<Optional<Draft>> CreateDraft(CreateDraftDto dto) => await httpCrudService.CreateAsync<Draft, CreateDraftDto>("drafts", dto);

    // GET DRAFTS
    public async Task<Optional<List<Draft>>> GetDrafts() => await httpCrudService.GetAsync<List<Draft>>("drafts");

    // APPROVE DRAFT
    public async Task<Optional<Draft>> ApproveDraft(int draftId, int adminId) => await httpCrudService.UpdateAsync<Draft, int>("drafts", adminId, draftId);

    // COURSE PROGRESS
    public async Task<Optional<int>> GetCourseProgressAsync(int userId, int courseId) => await httpCrudService.GetAsync<int>($"CourseProgress/{userId}/{courseId}");
    public async Task<Optional<int>> UpdateCourseProgressAsync(int userId, int courseId, int currentStep)
    {
        var dto = new { UserId = userId, CourseId = courseId, CurrentStep = currentStep };

        return await httpCrudService.CreateAsync<int, object>("CourseProgress", dto);
    }

    // UPDATE COURSE
    public async Task<Optional<Course>> UpdateCourse(int id, Course course) => await httpCrudService.UpdateAsync<Course, Course>($"courses/{id}", course);

    // LEADERBOARD
    public async Task<Optional<List<LeaderboardEntry>>> GetLeaderboardAsync() => await httpCrudService.GetAsync<List<LeaderboardEntry>>("leaderboard");

    // CATEGORIES
    public async Task<Optional<CourseCategory>> CreateCategory(CreateCourseCategoryDto dto) => await httpCrudService.CreateAsync<CourseCategory, CreateCourseCategoryDto>("categories", dto);
    public async Task<List<CourseCategory>> GetCategories() => (await httpCrudService.GetAsync<List<CourseCategory>>("categories")).Value ?? [];

    // LANGUAGES
    public async Task<Optional<Language>> CreateLanguage(CreateLanguageDto dto) => await httpCrudService.CreateAsync<Language, CreateLanguageDto>("languages", dto);
    public async Task<List<Language>> GetLanguages() => (await httpCrudService.GetAsync<List<Language>>("languages")).Value ?? [];

    public async Task DeleteCourseProgressAsync(int courseId, int userId)
    {

        var uri = $"CourseProgress/{courseId}/{userId}";

        HttpResponseMessage response = await client.DeleteAsync(uri);
        response.EnsureSuccessStatusCode();
    }
}
