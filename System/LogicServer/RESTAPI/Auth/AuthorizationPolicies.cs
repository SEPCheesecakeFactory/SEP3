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
                a.RequireAuthenticatedUser().RequireClaim("Role", "Learner"));
                
            options.AddPolicy("MustBeTeacher", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "Teacher"));

            options.AddPolicy("MustBeAdmin", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "Admin"));
        });
    }
}
