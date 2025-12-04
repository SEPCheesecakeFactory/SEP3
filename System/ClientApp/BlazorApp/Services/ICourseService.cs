using BlazorApp.Entities;

namespace BlazorApp.Services;

public interface ICourseService
{
    public Task<List<Course>> GetCourses();
    public Task<List<Course>> GetCourses(int? userId = null);
    public Task CreateDraft(CreateDraftDto dto);
    public Task<List<Draft>> GetDrafts();
    public Task ApproveDraft(int draftId, int adminId);

    // progress
    Task<int> GetCourseProgressAsync(int userId, int courseId);
    Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep);
}
