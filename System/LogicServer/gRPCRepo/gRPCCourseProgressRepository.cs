using RepositoryContracts;
using via.sep3.dataserver.grpc;
using Entities;
using System.Threading.Tasks;

namespace gRPCRepo; 

public class gRPCCourseProgressRepository(string host, int port) 
    : gRPCRepository<CourseProgress, CourseProgress, CourseProgress, string>(host, port), 
      ICourseProgressRepository
{    
    public async Task<int> GetCourseProgressAsync(int userId, int courseId)
    {
        var request = new CourseProgressRequest
        {
            UserId = userId,
            CourseId = courseId
        };
        CourseProgressResponse response = await ProgressServiceClient.GetCourseProgressAsync(request); 
        return response.CurrentStep;
    }

    public async Task UpdateCourseProgressAsync(int userId, int courseId, int currentStep)
    {
        var request = new CourseProgressUpdate
        {
            UserId = userId,
            CourseId = courseId,
            CurrentStep = currentStep
        };
        await ProgressServiceClient.UpdateCourseProgressAsync(request);
    }
    
    public override Task ClearAsync() => throw new NotImplementedException();
    public override Task DeleteAsync(string id) => throw new NotImplementedException();
    public override IQueryable<CourseProgress> GetMany() => throw new NotImplementedException();
    public override Task<CourseProgress> GetSingleAsync(string id) => throw new NotImplementedException();
    public override Task<CourseProgress> AddAsync(CourseProgress entity) => throw new NotImplementedException();
    public override Task<CourseProgress> UpdateAsync(CourseProgress entity) => throw new NotImplementedException();
}