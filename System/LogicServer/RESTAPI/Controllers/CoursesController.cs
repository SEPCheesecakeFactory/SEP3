using System;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly IRepository<Entities.Course> courseRrepository;
    public CoursesController(IRepository<Entities.Course> courseRepository)
    {
        this.courseRrepository = courseRepository;
    }

        [HttpGet, Authorize("MustBeTeacher")]
    public async Task<IResult> GetAllCourses()
    {
        var user = HttpContext.User;
        Console.WriteLine($"User: {user.Identity?.Name}, Claims: {string.Join(", ", user.Claims.Select(c => $"{c.Type}: {c.Value}"))}");
        var courses = courseRrepository.GetMany();
        return Results.Ok(courses);
    }


}
