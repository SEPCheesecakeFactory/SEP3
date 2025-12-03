using Entities;
using RepositoryContracts;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

[ApiController]
[Route("courses")]  
public class CoursesController : GenericController<Course, int>
{
    public CoursesController(IRepositoryID<Course, int> repository)
        : base(repository)
    {
    }
}
