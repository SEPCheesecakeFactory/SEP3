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
    public async Task<Optional<bool>> CreateDraft(CreateDraftDto dto) => await httpCrudService.CreateAsync<bool, CreateDraftDto>("drafts", dto);

    // GET DRAFTS
    public async Task<Optional<List<Draft>>> GetDrafts() => await httpCrudService.GetAsync<List<Draft>>("drafts");

    // APPROVE DRAFT
    public async Task<Optional<bool>> ApproveDraft(int draftId, int adminId)
    {
        try
        {
            var response = await httpCrudService.Client.PutAsJsonAsync($"drafts/{draftId}", adminId);

            if (!response.IsSuccessStatusCode)
            {
                var serverMessage = await response.Content.ReadAsStringAsync();

                var errorMessage = string.IsNullOrWhiteSpace(serverMessage)
                    ? $"Server error: {response.StatusCode}"
                    : serverMessage;

                return Optional<bool>.Error(errorMessage);
            }

            return Optional<bool>.Success(true);
        }
        catch (HttpRequestException)
        {
            return Optional<bool>.Error("NO_CONNECTION");
        }
        catch (Exception ex)
        {
            return Optional<bool>.Error("Unexpected error: " + ex.Message);
        }
    }

    // COURSE PROGRESS
    public async Task<Optional<int>> GetCourseProgressAsync(int userId, int courseId) => await httpCrudService.GetAsync<int>($"CourseProgress/{userId}/{courseId}");
    public async Task<Optional<bool>> UpdateCourseProgressAsync(int userId, int courseId, int currentStep)
    {
        var dto = new { UserId = userId, CourseId = courseId, CurrentStep = currentStep };

        return await httpCrudService.CreateAsync<bool, object>("CourseProgress", dto);
    }

    // UPDATE COURSE
    public async Task<Optional<bool>> UpdateCourse(int id, Course course) => await httpCrudService.UpdateAsync<bool, Course>($"courses/{id}", course);

    // LEADERBOARD
    public async Task<Optional<List<LeaderboardEntry>>> GetLeaderboardAsync() => await httpCrudService.GetAsync<List<LeaderboardEntry>>("leaderboard");

    // CATEGORIES
    public async Task CreateCategory(CreateCourseCategoryDto dto) => await httpCrudService.CreateAsync<CourseCategory, CreateCourseCategoryDto>("categories", dto);
    public async Task<List<CourseCategory>> GetCategories() => (await httpCrudService.GetAsync<List<CourseCategory>>("categories")).Value ?? [];

    // LANGUAGES
    public async Task CreateLanguage(CreateLanguageDto dto) => await httpCrudService.CreateAsync<Language, CreateLanguageDto>("languages", dto);
    public async Task<List<Language>> GetLanguages() => (await httpCrudService.GetAsync<List<Language>>("languages")).Value ?? [];
}
