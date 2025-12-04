using Grpc.Net.Client;
using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;

namespace gRPCRepo;

public class gRPCCourseRepository(string host, int port) : gRPCRepository<Entities.Course, int>(host, port)
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
            Category = c.Category
        }).ToList();

        return courses.AsQueryable();
    }

    public override Task<Entities.Course> AddAsync(Entities.Course entity)
    {
        throw new NotImplementedException();
    }

   public override async Task UpdateAsync(Entities.Course entity)
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

    await Client.UpdateCourseAsync(request);
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
}
