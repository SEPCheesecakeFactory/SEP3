using Entities;
using Grpc.Net.Client;
using via.sep3.dataserver.grpc; // Generated namespace
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RepositoryContracts;

namespace gRPCRepo;

public class gRPCLearningStepRepository(string host, int port, bool useTls = false) : gRPCRepository<Entities.LearningStep, (int, int)>(host, port, useTls)
{
    public override Task<Entities.LearningStep> AddAsync(Entities.LearningStep entity)
    {
        throw new NotImplementedException();
    }

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }

    public override Task DeleteAsync((int, int) id)
    {
        throw new NotImplementedException();
    }

    public override IQueryable<Entities.LearningStep> GetMany()
    {
        throw new NotImplementedException();
    }

    public override async Task<Entities.LearningStep> GetSingleAsync((int, int) id)
    {
        var (courseId, stepOrder) = id;
        
        var response = await Client.GetLearningStepAsync(new GetLearningStepRequest { CourseId = courseId, StepNumber = stepOrder }) ?? throw new KeyNotFoundException($"LearningStep with CourseId {courseId} and StepOrder {stepOrder} not found.");
        
        return new Entities.LearningStep
        {
            CourseId = response.LearningStep.CourseId,
            StepOrder = response.LearningStep.StepOrder,
            Type = response.LearningStep.Type,
            Content = response.LearningStep.Content
        };
    }

    public override Task UpdateAsync(Entities.LearningStep entity)
    {
        throw new NotImplementedException();
    }
}