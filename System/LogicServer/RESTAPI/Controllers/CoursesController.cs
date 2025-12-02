using Entities;
using RepositoryContracts;

namespace RESTAPI.Controllers;

public class CoursesController(IRepositoryID<Course, Course, Course, int> repository) : GenericDefaultController<Course, Course, Course, int>(repository)
{
    
}
