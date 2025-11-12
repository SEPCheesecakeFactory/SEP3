using System;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RESTAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly IRepository<Entities.Course> courseRrepository;
    public CoursesController(IRepository<Entities.Course> courseRepository)
    {
        this.courseRrepository = courseRepository;
    }

    [HttpGet]
    public async Task<IResult> GetAllCourses()
    {
        var courses = courseRrepository.GetMany();
        return Results.Ok(courses);
    }


}
