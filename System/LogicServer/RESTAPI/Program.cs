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
builder.Services.AddScoped<IRepository<Entities.User>>(sp => new gRPCUserRepository("localhost", 9090));

builder.Services.AddScoped<IAuthService, RESTAPI.Services.AuthService>();
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


// ===

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
// using (var scope = app.Services.CreateScope())
// {
//     var provider = scope.ServiceProvider;
//     var logger = provider.GetRequiredService<ILogger<Program>>();
//     var userRepo = provider.GetRequiredService<IRepository<Entities.User>>();
//     try
//     {
//         userRepo.ClearAsync().GetAwaiter().GetResult();
//         userRepo.AddAsync(new Entities.User { Username = "alice", Password = "password1", Role = "FakeLearner" }).GetAwaiter().GetResult();
//         userRepo.AddAsync(new Entities.User { Username = "bob", Password = "password2", Role = "Learner" }).GetAwaiter().GetResult();
//         logger.LogInformation("Seeded 2 test users for authentication: alice, bob");
//     }
//     catch (Exception ex)
//     {
//         logger.LogError(ex, "Error seeding in-memory users");
//     }
// }

// === RUN ===

app.Run();