using BlazorApp.Entities;

namespace BlazorApp.Services;

public interface ICourseService
{
    public Task<List<Course>> GetCourses();
    public Task CreateDraft(CreateDraftDto dto);
}
