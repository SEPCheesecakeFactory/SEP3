namespace BlazorApp.Utils;

/// <summary>
/// Utility class for course display styling helpers.
/// Shared between MyCourses and AllCourses pages.
/// </summary>
public static class CourseDisplayUtils
{
    public static string GetBgClass(string? category) => (category?.ToLower()) switch
    {
        "computer science" => "bg-blue-100",
        "math" => "bg-purple-100",
        "history" => "bg-yellow-100",
        "languages" => "bg-red-100",
        "science" => "bg-green-100",
        _ => "bg-blue-100"
    };

    public static string GetTextClass(string? category) => (category?.ToLower()) switch
    {
        "computer science" => "text-blue-600",
        "math" => "text-purple-600",
        "history" => "text-yellow-600",
        "languages" => "text-red-600",
        "science" => "text-green-600",
        _ => "text-blue-600"
    };

    public static string GetBorderClass(string? category) => (category?.ToLower()) switch
    {
        "computer science" => "border-blue-200",
        "math" => "border-purple-200",
        "history" => "border-yellow-200",
        "languages" => "border-red-200",
        "science" => "border-green-200",
        _ => "border-blue-200"
    };

    public static string GetIconClass(string? category) => (category?.ToLower()) switch
    {
        "computer science" => "fa-brands fa-git-alt",
        "software engineering" => "fa-brands fa-git-alt",
        "math" => "fas fa-calculator",
        "history" => "fas fa-landmark",
        "languages" => "fas fa-language",
        "science" => "fas fa-flask",
        _ => "fas fa-book"
    };
}
