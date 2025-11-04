using Grpc.Net.Client;
using Entities;
using RepositoryContracts;

namespace gRPCRepo;

public class gRPCCourseRepository : gRPCRepository<Entities.Course>
{
    public gRPCCourseRepository(string host, int port) : base(host, port)
    {
    }

    public override IQueryable<Entities.Course> GetMany()
    {
        var resp = Client.GetCourses(new GetCoursesRequest());
        var courses = resp.Courses.Select(c => new Entities.Course
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Language = c.Language,
            Category = c.Category
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
}
