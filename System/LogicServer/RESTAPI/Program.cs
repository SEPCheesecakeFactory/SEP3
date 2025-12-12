using gRPCRepo;
using RepositoryContracts;
using RESTAPI;
using RESTAPI.Services;
using RESTAPI.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// == GRPC STUFF ==
using Grpc.Net.Client;
using via.sep3.dataserver.grpc; // needed for gRPC user client
// == DOMAIN STUFF ==
using BlazorApp.Entities; // DTOs (CreateCourseDto, CreateLanguageDto, etc.) - note: it is just called Blazor.* but it is NOT part of the Blazor project
using Entities;

// ==================================
//   App builder
// ==================================

var builder = WebApplication.CreateBuilder(args);

// ==========================================
//   Controllers, middleware, and OpenAPI
// ==========================================

builder.Services.AddControllers();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// ==================================
//   External service endpoints
// ==================================

var host = "localhost";
var port = 9090;

// ==================================
//   gRPC client registrations
// ==================================

builder.Services.AddScoped<UserService.UserServiceClient>(sp =>
{
    var channel = GrpcChannel.ForAddress($"http://{host}:{port}");
    return new UserService.UserServiceClient(channel);
});

// ===========================================
//   Repository registrations (gRPC-backed)
// ===========================================

builder.Services.AddScoped<ILeaderboardRepository>(sp => new gRPCLeaderBoardEntryRepository(host, port));
builder.Services.AddScoped<ICourseProgressRepository>(sp => new gRPCCourseProgressRepository(host, port));
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

builder.Services.AddScoped<IRepositoryID<Entities.CourseCategory, CreateCourseCategoryDto, Entities.CourseCategory, int>>(sp => new gRPCCourseCategoryRepository(host, port));
builder.Services.AddScoped<IRepositoryID<Entities.Language, CreateLanguageDto, Entities.Language, string>>(sp => new gRPCLanguageRepository(host, port));
builder.Services.AddScoped<IRepositoryID<Entities.LearningStep, Entities.LearningStep, Entities.LearningStep, (int, int)>>(sp => new gRPCLearningStepRepository(host, port));

// =====================================
//   Authentication and authorization
// =====================================

builder.Services.AddScoped<IAuthService, SecureAuthService>();

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

// ==================================
//   Build app
// ==================================

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ==================================
//   Run
// ==================================

app.Run();