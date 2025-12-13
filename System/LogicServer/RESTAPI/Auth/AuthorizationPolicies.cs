using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace RESTAPI.Auth;

public static class AuthorizationPolicies
{
    public static void AddPolicies(IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("MustBeTeacher", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "teacher"));

            options.AddPolicy("MustBeAdmin", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "admin"));

            options.AddPolicy("MustBeTeacherOrAdmin", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "teacher", "admin"));
        });
    }
}
