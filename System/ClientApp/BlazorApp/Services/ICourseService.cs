using BlazorApp.Entities;

namespace BlazorApp.Services;

public interface ICourseService
{
    public Task<List<Course>> GetCourses();
    public Task CreateDraft(CreateDraftDto dto);
    public Task<List<Draft>> GetDrafts();
    public Task ApproveDraft(int draftId, int adminId);
}
