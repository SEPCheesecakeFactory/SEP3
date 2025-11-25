using Entities;
using Grpc.Net.Client;
using via.sep3.dataserver.grpc; // Generated namespace
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using RepositoryContracts;

namespace gRPCRepo;

public class gRPCLearningStepRepository : ILearningStepRepository
{
    private readonly DataRetrievalService.DataRetrievalServiceClient _client;

    public gRPCLearningStepRepository(string host, int port)
    {
        var channel = GrpcChannel.ForAddress($"http://{host}:{port}");
        _client = new DataRetrievalService.DataRetrievalServiceClient(channel);
    }

    public async Task<IEnumerable<Entities.LearningStep>> GetForCourseAsync(int courseId)
    {
        var request = new GetLearningStepsRequest { CourseId = courseId };
        var response = await _client.GetLearningStepsAsync(request);

        return response.LearningSteps.Select(ls => new Entities.LearningStep
        {
            CourseId = ls.CourseId,
            StepOrder = ls.StepOrder,
            Type = ls.Type,
            Content = ls.Content
        }).ToList();
    }

    public async Task<Entities.LearningStep> GetSingleAsync(int courseId, int stepOrder)
    {
        var request = new LearningStepKey 
        { 
            CourseId = courseId, 
            StepOrder = stepOrder 
        };
        
        var response = await _client.GetLearningStepAsync(request);

        return new Entities.LearningStep
        {
            CourseId = response.CourseId,
            StepOrder = response.StepOrder,
            Type = response.Type,
            Content = response.Content
        };
    }

    public async Task<Entities.LearningStep> AddAsync(Entities.LearningStep entity)
    {
        var protoObj = new via.sep3.dataserver.grpc.LearningStep
        {
            CourseId = entity.CourseId,
            Type = entity.Type,
            Content = entity.Content
            // StepOrder is usually calculated by server or sent as 0
        };

        var response = await _client.AddLearningStepAsync(protoObj);

        entity.StepOrder = response.StepOrder; // Server assigns the order
        return entity;
    }

    public async Task UpdateAsync(Entities.LearningStep entity)
    {
        var protoObj = new via.sep3.dataserver.grpc.LearningStep
        {
            CourseId = entity.CourseId,
            StepOrder = entity.StepOrder,
            Type = entity.Type,
            Content = entity.Content
        };

        await _client.UpdateLearningStepAsync(protoObj);
    }

    public async Task DeleteAsync(int courseId, int stepOrder)
    {
        var request = new LearningStepKey 
        { 
            CourseId = courseId, 
            StepOrder = stepOrder 
        };
        await _client.DeleteLearningStepAsync(request);
    }
}