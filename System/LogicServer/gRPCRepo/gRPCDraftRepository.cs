using Grpc.Net.Client;
using Entities;
using RepositoryContracts;
using via.sep3.dataserver.grpc;

namespace gRPCRepo;

public class gRPCDraftRepository(string host, int port) : gRPCRepository<Entities.Draft, int>(host, port)
{
    public override IQueryable<Entities.Draft> GetMany()
    {
        throw new NotImplementedException();
    }

    public override Task<Entities.Draft> AddAsync(Entities.Draft entity)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(Entities.Draft entity)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override async Task<Entities.Draft> GetSingleAsync(int id)
    {
        
        var response = await Client.GetDraftAsync(new GetDraftRequest { DraftId = id} ) ?? throw new KeyNotFoundException($"Draft with id {id} not found.");
        
        return new Entities.Draft
        {
            Id = response.CourseDraft.Id,
            Language = response.CourseDraft.Language,
            Title = response.CourseDraft.Title,
            Description = response.CourseDraft.Description,
            TeacherId = response.CourseDraft.TeacherId,
            CourseId = response.CourseDraft.CourseId
        };
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }
}
