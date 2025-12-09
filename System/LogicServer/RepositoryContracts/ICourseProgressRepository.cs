namespace RepositoryContracts;

public interface ICourseProgressRepository 
{
    Task<int> GetCourseProgressAsync(int userId, int courseId); 
    Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep);
    
}