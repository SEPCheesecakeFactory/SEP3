using Entities; // Your generic models
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface ICourseRepository : IRepositoryID<Course, CreateCourseDto, Course, int>
    {
        Task<int> GetCourseProgressAsync(int userId, int courseId);  
        Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep);
        IQueryable<Course> GetManyByUserId(int? userId = null);
        Task DeleteAsync(int courseId, int userId);
    }
}