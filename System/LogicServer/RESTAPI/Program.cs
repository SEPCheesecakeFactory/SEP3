using Entities;
using gRPCRepo;
using InMemoryRepositories;
using RepositoryContracts;
using RESTAPI.Controllers;
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

// === RUN ===

app.Run();