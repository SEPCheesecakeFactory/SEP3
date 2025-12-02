using Grpc.Net.Client;
using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;
using Course = Entities.Course;

namespace gRPCRepo;

public class gRPCCourseRepository(string host, int port) : gRPCRepository<Course, Course, Course, int>(host, port)
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

    public override Task<Course> AddAsync(Course entity)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(Course entity)
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
