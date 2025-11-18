using Entities;
using gRPCRepo;
using InMemoryRepositories;
using RepositoryContracts;
using RESTAPI;
using RESTAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

// === In-Memory Repositories for Testing ===

// var dummyCourseRepo = new InMemoryRepository<Course>();
// dummyCourseRepo.AddAsync(new Course { Id = 0, Title = "Introduction to Programming", Description = "Learn the basics of programming.", Language = "English" }).Wait();


builder.Services.AddScoped<IRepository<Entities.Course>>(sp => new gRPCCourseRepository("localhost", 9090));

// Register in-memory user repository for testing and seed data
// builder.Services.AddSingleton<IRepository<Entities.User>, InMemoryRepository<Entities.User>>();
builder.Services.AddScoped<IRepository<Entities.User>>(sp => new gRPCUserRepository("localhost",9090));

builder.Services.AddScoped<IAuthService, RESTAPI.Services.AuthService>();

// ===

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Seed test users into the in-memory repository so the Blazor app can log in
using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    var logger = provider.GetRequiredService<ILogger<Program>>();
    var userRepo = provider.GetRequiredService<IRepository<Entities.User>>();
    try
    {
        userRepo.ClearAsync().GetAwaiter().GetResult();
        userRepo.AddAsync(new Entities.User { Username = "alice", Password = "password1", Role = "FakeLearner" }).GetAwaiter().GetResult();
        userRepo.AddAsync(new Entities.User { Username = "bob", Password = "password2", Role = "Learner" }).GetAwaiter().GetResult();
        logger.LogInformation("Seeded 2 test users for authentication: alice, bob");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error seeding in-memory users");
    }
}

// === RUN ===

app.Run();