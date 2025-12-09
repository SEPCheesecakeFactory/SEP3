using BlazorApp.Entities;
using Entities; // Your generic models
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface ICourseCategoryRepository : IRepositoryID<CourseCategory, CreateCourseCategoryDto, CourseCategory, int>
    {
        
    }
}