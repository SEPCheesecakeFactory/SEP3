namespace RepositoryContracts;

public interface ICourseProgressRepository 
{
    Task<int> GetCourseProgressAsync(int userId, int courseId); 
    Task<int> UpdateCourseProgressAsync(int userId, int courseId, int currentStep);
    Task DeleteAsync(int courseId, int userId);
}