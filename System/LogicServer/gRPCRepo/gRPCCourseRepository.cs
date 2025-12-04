using Grpc.Net.Client;
using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;
using Course = Entities.Course;

namespace gRPCRepo;

public class gRPCCourseRepository(string host, int port) : gRPCRepository<Course, CreateCourseDto, Course, int>(host, port), ICourseRepository
{
    public override IQueryable<Course> GetMany()
    {
        // Hardcode the default behavior (e.g., 0 for public courses)
        var request = new GetCoursesRequest();
        return FetchCoursesRequestFromGrpc(request);
    }
    public IQueryable<Course> GetManyByUserId(int? userId = null)
    {
        var request = new GetCoursesRequest { UserId = userId ?? 0 };
        return FetchCoursesRequestFromGrpc(request);
    }

    private IQueryable<Course> FetchCoursesRequestFromGrpc(GetCoursesRequest request)
    {
        var resp = Client.GetCourses(request);

        return resp.Courses.Select(c => new Course
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Language = c.Language,
            Category = c.Category,
            TotalSteps = c.TotalSteps
        }).AsQueryable();
    }

    public override async Task<Course> AddAsync(CreateCourseDto entity)
    {
        var request = new AddCourseRequest
        {
            Title = entity.Title ?? "",
            Description = entity.Description ?? "",
            Language = entity.Language ?? "",
            Category = entity.Category ?? ""
        };

        var response = await Client.AddCourseAsync(request);

        return new Course
        {
            Id = response.Course.Id,
            Title = response.Course.Title,
            Description = response.Course.Description,
            Language = response.Course.Language,
            Category = response.Course.Category
        };
    }

    public override async Task<Course> UpdateAsync(Course entity)
    {
        var request = new UpdateCourseRequest
        {
            Course = new via.sep3.dataserver.grpc.Course
            {
                Id = entity.Id,
                Title = entity.Title ?? "",
                Description = entity.Description ?? "",
                Language = entity.Language ?? "",
                Category = entity.Category ?? ""
            }
        };

        var response = await Client.UpdateCourseAsync(request);

        return new Course
        {
            Id = response.Course.Id,
            Title = response.Course.Title,
            Description = response.Course.Description,
            Language = response.Course.Language,
            Category = response.Course.Category
        };
    }


    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override async Task<Entities.Course> GetSingleAsync(int id)
    {
        // gRPC has no GetCourseById, so we fetch all and find it (like repository.Memory)
        var resp = Client.GetCourses(new GetCoursesRequest());

        var c = resp.Courses.FirstOrDefault(c => c.Id == id);

        if (c == null)
            throw new KeyNotFoundException($"Course {id} not found");

        return new Entities.Course
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Language = c.Language,
            Category = c.Category
        };
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

    public Task<Course> AddAsync(Course entity)
    {
        throw new NotImplementedException();
    }
}
