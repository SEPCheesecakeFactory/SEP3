using Grpc.Net.Client;
using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;
using Course = Entities.Course;

namespace gRPCRepo;

public class gRPCCourseRepository(string host, int port) : gRPCRepository<Course, CreateCourseDto, Course, int>(host, port)
{
    public override IQueryable<Course> GetMany()
    {
        var resp = Client.GetCourses(new GetCoursesRequest());
        var courses = resp.Courses.Select(c => new Course
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Language = c.Language,
            Category = c.Category
        }).ToList();

        return courses.AsQueryable();
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

    public override Task<Course> UpdateAsync(Course entity)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task<Course> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }
}
