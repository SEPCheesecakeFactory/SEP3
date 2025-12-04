using Grpc.Net.Client;
using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;

namespace gRPCRepo;

public class gRPCCourseRepository(string host, int port) : gRPCRepository<Entities.Course, int>(host, port), ICourseRepository
{
    public override IQueryable<Entities.Course> GetMany()
    {
        var resp = Client.GetCourses(new GetCoursesRequest());
        var courses = resp.Courses.Select(c => new Entities.Course
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Language = c.Language,
            Category = c.Category,
            TotalSteps = c.TotalSteps
        }).ToList();

        return courses.AsQueryable();
    }

    public override Task<Entities.Course> AddAsync(Entities.Course entity)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(Entities.Course entity)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task<Entities.Course> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetCourseProgressAsync(int userId, int courseId)
    {
        try
        {
            // Create Request
            var request = new CourseProgressRequest
            {
                UserId = userId,
                CourseId = courseId
            };

            // Call Java
            CourseProgressResponse response = await Client.GetCourseProgressAsync(request);

            // Return Step
            return response.CurrentStep;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw; 
        }
    }

    public async Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep)
    {
        try
        {
            // Create Request
            var request = new CourseProgressUpdate
            {
                UserId = userId,
                CourseId = courseId,
                CurrentStep = currentStep
            };

            // Call Java (Wait for Empty response)
            await Client.UpdateCourseProgressAsync(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
