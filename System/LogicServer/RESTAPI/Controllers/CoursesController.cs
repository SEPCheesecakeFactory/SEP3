using Entities;
using RepositoryContracts;

namespace RESTAPI.Controllers;

public class CoursesController(IRepositoryID<Course, int> repository) : GenericController<Course, int>(repository)
{
    
}
