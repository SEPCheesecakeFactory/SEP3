using Entities;
using gRPCRepo;
using InMemoryRepositories;
using RepositoryContracts;
using RESTAPI;
using RESTAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RESTAPI.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using RESTAPI.Controllers;

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

builder.Services.AddScoped<ILeaderboardRepository>(sp => new gRPCLeaderBoardEntryRepository(host, port));
builder.Services.AddScoped<IRepositoryID<Course, CreateCourseDto, Course, int>>(sp => new gRPCCourseRepository(host, port));
builder.Services.AddScoped<ICourseRepository>(sp => new gRPCCourseRepository(host, port));

// Register in-memory user repository for testing and seed data
// builder.Services.AddSingleton<IRepository<Entities.User>, InMemoryRepository<Entities.User>>();
builder.Services.AddScoped<IRepositoryID<User, User, User, int>>(sp => new gRPCUserRepository(host, port));
builder.Services.AddScoped<IAuthService, SecureAuthService>();
builder.Services.AddScoped<IRepositoryID<LearningStep, LearningStep, LearningStep, (int, int)>>(sp =>
    new gRPCLearningStepRepository(host, port));
builder.Services.AddScoped<IRepositoryID<Draft, CreateDraftDto, Draft, int>>(sp => new gRPCDraftRepository(host, port));
builder.Services.AddScoped<ICourseRepository>(sp => new gRPCCourseRepository(host, port));
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")),
        ClockSkew = TimeSpan.Zero,
    };
});
AuthorizationPolicies.AddPolicies(builder.Services);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

/*
using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    var logger = provider.GetRequiredService<ILogger<Program>>();
    var userRepo = provider.GetRequiredService<IRepository<Entities.User>>();
    try
    {
        userRepo.ClearAsync().GetAwaiter().GetResult();
        userRepo.AddAsync(new Entities.User { Username = "alice", Password = "password1", Roles = new List<Entities.Role> { new Entities.Role { RoleName = "Teacher" } } }).GetAwaiter().GetResult();
        userRepo.AddAsync(new Entities.User { Username = "bob", Password = "password2", Roles = new List<Entities.Role> { new Entities.Role { RoleName = "Learner" } } }).GetAwaiter().GetResult();
        logger.LogInformation("Seeded 2 test users for authentication: alice, bob");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error seeding in-memory users");
    }
}*/

// === RUN ===

app.Run();