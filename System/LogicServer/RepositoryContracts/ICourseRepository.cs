using Entities;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface ICourseRepository : IRepositoryID<Course, CreateCourseDto, Course, int>
    {
        IQueryable<Course> GetManyByUserId(int? userId = null);
    }
}