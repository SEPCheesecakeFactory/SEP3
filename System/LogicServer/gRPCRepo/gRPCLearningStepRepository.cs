using Grpc.Net.Client;
using Entities;
using RepositoryContracts;

namespace gRPCRepo;

public class gRPCLearningStepRepository : gRPCRepository<Entities.LearningStep>
{
    public gRPCLearningStepRepository(string host, int port) : base(host, port)
    {
    }

    public override IQueryable<Entities.LearningStep> GetMany()
    {
        var resp = Client.GetLearningSteps(new GetLearningStepsRequest());
        var learningSteps = resp.LearningSteps.Select(ls => new Entities.LearningStep
        {
            Id = ls.Id,
            Type = ls.Type,
            Content = ls.Content,
            CourseId = ls.CourseId
        }).ToList();

        return learningSteps.AsQueryable();
    }

    public override Task<Entities.LearningStep> AddAsync(Entities.LearningStep entity)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(Entities.LearningStep entity)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task<Entities.LearningStep> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }
}
