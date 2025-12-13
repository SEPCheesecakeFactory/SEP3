using Grpc.Net.Client;
using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;
using Course = Entities.Course;

namespace gRPCRepo;

public class gRPCCourseRepository : gRPCRepository<Course, CreateCourseDto, Course, int>, ICourseRepository
{
    private readonly string _host;
    private readonly int _port;

    private readonly UserService.UserServiceClient _userClient;

    public gRPCCourseRepository(string host, int port, UserService.UserServiceClient userClient) : base(host, port)
    {
        _host = host;
        _port = port;
        _userClient = userClient;
    }

    public override IQueryable<Course> GetMany()
    {
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
        // Get raw courses
        var resp = CourseServiceClient.GetCourses(request);
        var resultList = new List<Course>();

        foreach (var c in resp.Courses)
        {
            string authorName = "Unknown";

            if (c.AuthorId > 0)
            {
                try
                {
                    // No more channel creation, just uses the client.
                    var userResponse = _userClient.GetUser(new GetUserRequest { Id = c.AuthorId });
                    authorName = userResponse.Username;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Could not fetch author for Course {c.Id}. Error: {ex.Message}");
                }
            }

            resultList.Add(new Course
            {
                Id = c.Id,
                Title = c.Title ?? "Untitled",
                Description = c.Description ?? "",
                Language = c.Language ?? "English",
                Category = string.IsNullOrEmpty(c.Category) ? "General" : c.Category,
                TotalSteps = c.TotalSteps,
                AuthorId = c.AuthorId,
                ApprovedBy = c.ApprovedBy,
                AuthorName = authorName
            });
        }

        return resultList.AsQueryable();
    }

    public override async Task<Entities.Course> GetSingleAsync(int id)
    {
        var resp = CourseServiceClient.GetCourses(new GetCoursesRequest());
        var c = resp.Courses.FirstOrDefault(c => c.Id == id);

        if (c == null) throw new NotFoundException($"Course {id} not found");

        string authorName = "Unknown";
        if (c.AuthorId > 0)
        {
            try
            {
                var userResponse = _userClient.GetUser(new GetUserRequest { Id = c.AuthorId });
                authorName = userResponse.Username;
            }
            catch { }
        }

        return new Entities.Course
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Language = c.Language,
            Category = c.Category,
            AuthorId = c.AuthorId,
            ApprovedBy = c.ApprovedBy,
            TotalSteps = c.TotalSteps,
            AuthorName = authorName
        };
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

    public override Task DeleteAsync(int id) => throw new NotImplementedException();
    public override Task ClearAsync() => throw new NotImplementedException();
    public Task<Course> AddAsync(Course entity) => throw new NotImplementedException();
}