using System;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace RESTAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly IRepository<Course> courseRrepository;
    public CoursesController(IRepository<Course> courseRepository)
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
