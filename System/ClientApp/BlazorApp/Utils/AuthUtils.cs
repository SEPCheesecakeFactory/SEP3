using System;
using System.Security.Claims;
using System.Text.Json;
using BlazorApp.Entities;

namespace BlazorApp.Utils;

public static class AuthUtils
{
    const string ROLE_TEACHER = "teacher";
    const string ROLE_ADMIN = "admin";
     public static IEnumerable<string> ConvertFromClaim(string jsonInput)
    {
        if (jsonInput == null)
            return new List<string>();
            return [];
        try
        {
            List<string>? resultList = JsonSerializer.Deserialize<List<string>>(jsonInput);
            if (resultList != null)
            {
                return resultList;
            }
            else
            {
                return [jsonInput];
            }
        }
        catch (Exception)
        {
            return [jsonInput];
        }
    }

    public static bool HasRole(this IEnumerable<string> roles, string role)
    {
        return roles.Contains(role);
    }

    public static bool IsRole(this ClaimsPrincipal user, string role)
    {
        return user.Claims.Where(c => c.Type == "Role").Select(c => AuthUtils.ConvertFromClaim(c.Value).Single()).HasRole(role);
    }

    public static bool IsTeacher(this ClaimsPrincipal user)
    {
        return IsRole(user, ROLE_TEACHER);
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return IsRole(user, ROLE_ADMIN);
    }

    public static bool IsTeacherOrAdmin(this ClaimsPrincipal user)
    {
        return IsTeacher(user) || IsAdmin(user);
    }

    public static int? GetID(this ClaimsPrincipal user)
    {
        var idClaim = user.Claims.FirstOrDefault(c => c.Type == "Id");
        if (idClaim != null && int.TryParse(idClaim.Value, out int id))
        {
            return id;
        }
        return null;
    }

    public static bool IsTeacher(this User user) => user.Roles.Any(r => r.RoleName == ROLE_TEACHER);
    public static bool IsAdmin(this User user) => user.Roles.Any(r => r.RoleName == ROLE_ADMIN); 
    public static bool IsTeacherOrAdmin(this User user) => user.IsTeacher() || user.IsAdmin();   
}