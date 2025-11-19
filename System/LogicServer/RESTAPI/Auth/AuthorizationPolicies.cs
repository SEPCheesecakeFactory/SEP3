using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace RESTAPI.Auth;

public static class AuthorizationPolicies
{
    public static void AddPolicies(IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("MustBeLearner", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "learner"));

            options.AddPolicy("MustBeTeacher", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "teacher"));

            options.AddPolicy("MustBeAdmin", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "admin"));

            options.AddPolicy("x", a =>
            a.RequireAuthenticatedUser().RequireClaim("Username", "admin_user"));
        });
    }
}
