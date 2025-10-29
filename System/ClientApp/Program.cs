using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

// --- Domain Model ---
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = "en";
}

// --- Main Program ---
class Program
{
    static async Task Main()
    {
        using var httpClient = new HttpClient();

        //  Replace this later with Logic Server
        string url = "https://example.com/api/courses";

        Console.WriteLine("Example Courses...\n");

        try
        {
            //  Network call (commented for now)
            // var courses = await httpClient.GetFromJsonAsync<List<Course>>(url);

            //  Local test data instead of network call
            var courses = new List<Course>
            {
                new Course { Id = 1, Title = "Intro to C#", Description = "Learn the basics of C#", Language = "en" },
                new Course { Id = 2, Title = "Java for Beginners", Description = "Start coding in Java", Language = "en" },
                new Course { Id = 3, Title = "Web Development", Description = "HTML, CSS, and JavaScript fundamentals", Language = "en" }
            };

            if (courses == null || courses.Count == 0)
            {
                Console.WriteLine("No courses available.");
                return;
            }

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nCourses:\n");
                foreach (var course in courses)
                {
                    Console.WriteLine($"{course.Id}. {course.Title} ({course.Language})");
                }

                Console.WriteLine("\nEnter course ID to view details or type 'exit' to quit:");
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    exit = true;
                    Console.WriteLine("Goodbye!");
                }
                else if (int.TryParse(input, out int id))
                {
                    var selected = courses.Find(c => c.Id == id);
                    if (selected != null)
                    {
                        Console.WriteLine("\n--- Course Details ---");
                        Console.WriteLine($"ID: {selected.Id}");
                        Console.WriteLine($"Title: {selected.Title}");
                        Console.WriteLine($"Description: {selected.Description}");
                        Console.WriteLine($"Language: {selected.Language}");
                        Console.WriteLine(new string('-', 40));
                    }
                    else
                    {
                        Console.WriteLine("Course not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid ID or 'exit'.");
                }
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Error: {ex.Message}");
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine($"Response format not supported: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
