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
        var resp = CourseServiceClient.GetCourses(request);

        return resp.Courses.Select(c => new Course
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Language = c.Language,
            Category = c.Category,
            TotalSteps = c.TotalSteps,
            AuthorId = c.AuthorId,
            ApprovedBy = c.ApprovedBy
        }).AsQueryable();
    }

    public override async Task<Course> AddAsync(CreateCourseDto entity)
    {
        var request = new AddCourseRequest
        {
            Title = entity.Title ?? "",
            Description = entity.Description ?? "",
            Language = entity.Language ?? "",
            Category = entity.Category ?? "",
            AuthorId = entity.AuthorId ?? -1
        };

        var response = await CourseServiceClient.AddCourseAsync(request);

        return new Course
        {
            Id = response.Course.Id,
            Title = response.Course.Title,
            Description = response.Course.Description,
            Language = response.Course.Language,
            Category = response.Course.Category,
            AuthorId = response.Course.AuthorId,
            ApprovedBy = response.Course.ApprovedBy
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
                Category = entity.Category ?? "",
                AuthorId = entity.AuthorId ?? -1,
                ApprovedBy = entity.ApprovedBy ?? -1
            }
        };

        var response = await CourseServiceClient.UpdateCourseAsync(request);

        return new Course
        {
            Id = response.Course.Id,
            Title = response.Course.Title,
            Description = response.Course.Description,
            Language = response.Course.Language,
            Category = response.Course.Category,
            AuthorId = response.Course.AuthorId,
            ApprovedBy = response.Course.ApprovedBy,
            TotalSteps = response.Course.TotalSteps

        };
    }

    public override Task DeleteAsync(int id)//Delete course entirely
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int courseId, int userId) //Delete progress of the user in a course (unenroll)
    {
        try
        {
        await CourseServiceClient.DeleteCourseAsync(new DeleteCourseRequest { CourseId = courseId, UserId = userId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<Entities.Course> GetSingleAsync(int id)
    {
        // gRPC has no GetCourseById, so we fetch all and find it (like repository.Memory)
        var resp = CourseServiceClient.GetCourses(new GetCoursesRequest());

        var c = resp.Courses.FirstOrDefault(c => c.Id == id);

        if (c == null)
            throw new KeyNotFoundException($"Course {id} not found");

        return new Entities.Course
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Language = c.Language,
            Category = c.Category,
            AuthorId = c.AuthorId,
            ApprovedBy = c.ApprovedBy,
            TotalSteps = c.TotalSteps
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
            CourseProgressResponse response = await ProgressServiceClient.GetCourseProgressAsync(request);

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
            await ProgressServiceClient.UpdateCourseProgressAsync(request);
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
