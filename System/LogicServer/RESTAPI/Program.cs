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

var host = "localhost";
var port = 9090;

builder.Services.AddScoped<IRepositoryID<Course, int>>(sp => 
    new gRPCCourseRepository(host, port));

builder.Services.AddScoped<IRepositoryID<LearningStep, (int, int)>>(sp => 
    new gRPCLearningStepRepository(host, port));
    
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// === Simple map for GetCourses ===

app.MapGet("/courses", (IRepositoryID<Course, int> courseRepo) =>
{
    var courses = courseRepo.GetMany();
    return Results.Ok(courses);
});

app.MapGet("/getlearningstep/{id1}/{id2}", (IRepositoryID<LearningStep, (int, int)> learningStepRepo, int id1, int id2) =>
{
    LearningStep learningStep;
    try
    {
        learningStep = learningStepRepo.GetSingleAsync(new (id1, id2)).Result;
    }
    catch (NotFoundException e)
    {
        return Results.NotFound(e.Message);
    }    
    return Results.Ok(learningStep);
});

// === RUN ===

app.Run();