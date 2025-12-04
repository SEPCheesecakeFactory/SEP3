using Entities; // Your generic models
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<int> GetCourseProgressAsync(int userId, int courseId);  
        Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep);
    }
}