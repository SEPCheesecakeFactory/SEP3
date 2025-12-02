using Entities;
using Grpc.Net.Client;
using via.sep3.dataserver.grpc; // Generated namespace
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RepositoryContracts;
using LearningStep = Entities.LearningStep;

namespace gRPCRepo;

public class gRPCLearningStepRepository(string host, int port, bool useTls = false) : gRPCRepository<LearningStep, LearningStep, LearningStep, (int, int)>(host, port, useTls)
{
    public override Task<LearningStep> AddAsync(LearningStep entity)
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

    public override IQueryable<LearningStep> GetMany()
    {
        throw new NotImplementedException();
    }

    public override async Task<LearningStep> GetSingleAsync((int, int) id)
    {
        var (courseId, stepOrder) = id;

        var response = await Client.GetLearningStepAsync(new GetLearningStepRequest { CourseId = courseId, StepNumber = stepOrder }) ?? throw new KeyNotFoundException($"LearningStep with CourseId {courseId} and StepOrder {stepOrder} not found.");

        return new LearningStep
        {
            CourseId = response.LearningStep.CourseId,
            StepOrder = response.LearningStep.StepOrder,
            Type = response.LearningStep.Type,
            Content = response.LearningStep.Content
        };
    }

    public override Task UpdateAsync(LearningStep entity)
    {
        var request = new UpdateLearningStepRequest
        {
            LearningStep = new via.sep3.dataserver.grpc.LearningStep
            {
                CourseId = entity.CourseId,
                StepOrder = entity.StepOrder,
                Content = entity.Content,
                Type = entity.Type
            }
        };

        return Client.UpdateLearningStepAsync(request).ResponseAsync;
    }
}