using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using BlazorApp.Entities;

namespace BlazorApp.Utils;

public static class AuthUtils
{
    // === NOTE/WARNING ===============================================
    // Code Smell (stringly typed): Role names as constants
    // Using enum could be better but depends on flexibility needs
    //    - Eduard
    // ================================================================
    
    const string ROLE_TEACHER = "teacher";
    const string ROLE_ADMIN = "admin";

    public static IEnumerable<string> ConvertFromClaim(string jsonInput)
    {
        if (string.IsNullOrEmpty(jsonInput))
            return [];

        try
        {
            // Try to parse as a JSON array (e.g. ["teacher", "admin"])
            if (jsonInput.Trim().StartsWith('['))
            {
                var result = JsonSerializer.Deserialize<List<string>>(jsonInput);
                return result ?? [];
            }
            // If it's not a JSON array, treat it as a single string
            return [jsonInput];
        }
        catch
        {
            // If parsing fails, assume it's a simple string
            return [jsonInput];
        }
    }

    public static bool IsRole(this ClaimsPrincipal user, string role)
    {
        if (user == null || !user.Identity.IsAuthenticated) return false;

        // 1. Check standard ClaimTypes.Role
        var standardRoles = user.FindAll(ClaimTypes.Role).Select(c => c.Value);

        // 2. Check custom "Role" claim and parse JSON if necessary
        var customRoles = user.FindAll("Role")
                              .SelectMany(c => ConvertFromClaim(c.Value));

        // Combine all found roles
        var allRoles = standardRoles.Concat(customRoles);

        // Check if the specific role exists (ignoring case)
        return allRoles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
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
        var idClaim = user.FindFirst("Id") ?? user.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim != null && int.TryParse(idClaim.Value, out int id))
        {
            return id;
        }
        return null;
    }

    // Entity helpers
    public static bool IsTeacher(this User user) => user.Roles.Any(r => r.RoleName == ROLE_TEACHER);
    public static bool IsAdmin(this User user) => user.Roles.Any(r => r.RoleName == ROLE_ADMIN);
    public static bool IsTeacherOrAdmin(this User user) => user.IsTeacher() || user.IsAdmin();
}