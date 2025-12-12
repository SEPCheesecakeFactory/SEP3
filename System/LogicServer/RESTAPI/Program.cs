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
using via.sep3.dataserver.grpc; // Needed for UserService
using Grpc.Net.Client;
using BlazorApp.Entities; // Added for GrpcChannel

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

// reg grpc client
builder.Services.AddScoped<UserService.UserServiceClient>(sp =>
{
    var channel = GrpcChannel.ForAddress($"http://{host}:{port}");
    return new UserService.UserServiceClient(channel);
});

// reg repositories (fixed ambiguities)
builder.Services.AddScoped<ILeaderboardRepository>(sp => new gRPCLeaderBoardEntryRepository(host, port));
builder.Services.AddScoped<ICourseProgressRepository>(sp => new gRPCCourseProgressRepository(host, port));
// Not valid builder.Services.AddScoped<IRepositoryID<Course, CreateCourseDto, Course, int>>(sp => new gRPCCourseRepository(host, port));
// Not valid builder.Services.AddScoped<ICourseRepository>(sp => new gRPCCourseRepository(host, port));
builder.Services.AddScoped<IRepositoryID<Entities.User, Entities.User, Entities.User, int>>(sp => new gRPCUserRepository(host, port));

builder.Services.AddScoped<ICourseRepository>(sp =>
{
    var userClient = sp.GetRequiredService<UserService.UserServiceClient>();
    return new gRPCCourseRepository(host, port, userClient);
});

builder.Services.AddScoped<IRepositoryID<Entities.Course, CreateCourseDto, Entities.Course, int>>(sp =>
{
    var userClient = sp.GetRequiredService<UserService.UserServiceClient>();
    return new gRPCCourseRepository(host, port, userClient);
});

builder.Services.AddScoped<IAuthService, SecureAuthService>();


builder.Services.AddScoped<IRepositoryID<Entities.CourseCategory, CreateCourseCategoryDto, Entities.CourseCategory, int>>(sp => new gRPCCourseCategoryRepository(host, port));
builder.Services.AddScoped<IRepositoryID<Entities.Language, CreateLanguageDto, Entities.Language, string>>(sp => new gRPCLanguageRepository(host, port));

// Register in-memory user repository for testing and seed data
// builder.Services.AddSingleton<IRepository<Entities.User>, InMemoryRepository<Entities.User>>();
// not valid now builder.Services.AddScoped<IRepositoryID<User, User, User, int>>(sp => new gRPCUserRepository(host, port));
builder.Services.AddScoped<IRepositoryID<Entities.Course, CreateCourseDto, Entities.Course, int>>(sp =>
{
    var userClient = sp.GetRequiredService<UserService.UserServiceClient>();
    return new gRPCCourseRepository(host, port, userClient);
});
builder.Services.AddScoped<IAuthService, SecureAuthService>();
//not valid now builder.Services.AddScoped<IRepositoryID<LearningStep, LearningStep, LearningStep, (int, int)>>(sp =>  new gRPCLearningStepRepository(host, port));
// now 'Entities.LearningStep' to avoid conflict with gRPC 'LearningStep'
builder.Services.AddScoped<IRepositoryID<Entities.LearningStep, Entities.LearningStep, Entities.LearningStep, (int, int)>>(sp =>
    new gRPCLearningStepRepository(host, port));
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

        RoleClaimType = "Role",
        NameClaimType = "Username"
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