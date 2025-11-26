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
    public override async Task<Entities.LearningStep> GetSingleAsync((int, int) id)
    {
        var (courseId, stepOrder) = id;

        var request = new LearningStepKey 
        { 
            CourseId = courseId, 
            StepOrder = stepOrder 
        };
        
        var response = await Client.GetLearningStepAsync(request) ?? throw new KeyNotFoundException($"LearningStep with CourseId {courseId} and StepOrder {stepOrder} not found.");
        
        return new Entities.LearningStep
        {
            CourseId = response.CourseId,
            StepOrder = response.StepOrder,
            Type = response.Type,
            Content = response.Content
        };
    }

    public override async Task<Entities.LearningStep> AddAsync(Entities.LearningStep entity)
    {
        var protoObj = new via.sep3.dataserver.grpc.LearningStep
        {
            CourseId = entity.CourseId,
            Type = entity.Type,
            Content = entity.Content
            // StepOrder is usually calculated by server or sent as 0
        };

        var response = await Client.AddLearningStepAsync(protoObj);

        entity.StepOrder = response.StepOrder; // Server assigns the order
        return entity;
    }

    public override async Task UpdateAsync(Entities.LearningStep entity)
    {
        var protoObj = new via.sep3.dataserver.grpc.LearningStep
        {
            CourseId = entity.CourseId,
            StepOrder = entity.StepOrder,
            Type = entity.Type,
            Content = entity.Content
        };

        await Client.UpdateLearningStepAsync(protoObj);
    }

    public override async Task DeleteAsync((int, int) id)
    {
        var (courseId, stepOrder) = id;
    {
        var request = new LearningStepKey 
        { 
            CourseId = courseId, 
            StepOrder = stepOrder 
        };
        await Client.DeleteLearningStepAsync(request);
    }
}

    public override Task ClearAsync()
    {
        throw new NotImplementedException();
    }

    public override IQueryable<Entities.LearningStep> GetMany()
    {
        throw new NotImplementedException();
    }
}