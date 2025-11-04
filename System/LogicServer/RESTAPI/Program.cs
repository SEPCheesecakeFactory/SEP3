using Entities;
using gRPCRepo;
using InMemoryRepositories;
using RepositoryContracts;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

// === In-Memory Repositories for Testing ===

// var dummyCourseRepo = new InMemoryRepository<Course>();
// dummyCourseRepo.AddAsync(new Course { Id = 0, Title = "Introduction to Programming", Description = "Learn the basics of programming.", Language = "English" }).Wait();


builder.Services.AddScoped<IRepository<Entities.Course>>(sp => new gRPCCourseRepository("localhost", 9090));

// ===

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// === Simple map for GetCourses ===

app.MapGet("/courses", (IRepository<Entities.Course> courseRepo) =>
{
    var courses = courseRepo.GetMany();
    return Results.Ok(courses);
});

// === RUN ===

app.Run();