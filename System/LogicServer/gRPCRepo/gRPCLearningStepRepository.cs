// System/LogicServer/gRPCRepo/LearningStepGrpcRepository.cs
using Entities;
using Grpc.Net.Client;
using via.sep3.dataserver.grpc; // Namespace generated from proto
using System.Linq;
using System.Threading.Tasks;

namespace gRPCRepo;

public class gRPCLearningStepRepository : gRPCRepository<Entities.LearningStep>
{
    public gRPCLearningStepRepository(string host, int port) : base(host, port)
    {
    }

    // 1. GET MANY
    public override IQueryable<Entities.LearningStep> GetMany()
    {
        // Call gRPC
        var response = Client.GetLearningSteps(new GetLearningStepsRequest());

        // Map Protobuf -> Entity
        var learningSteps = response.LearningSteps.Select(ls => new Entities.LearningStep
        {
            Id = ls.Id,
            Type = ls.Type,
            Content = ls.Content,
            CourseId = ls.CourseId
        }).ToList();

        return learningSteps.AsQueryable();
    }

    // 2. GET SINGLE
    public override async Task<Entities.LearningStep> GetSingleAsync(int id)
    {
        var request = new IdRequest { Id = id };
        var response = await Client.GetLearningStepAsync(request);

        return new Entities.LearningStep
        {
            Id = response.Id,
            Type = response.Type,
            Content = response.Content,
            CourseId = response.CourseId
        };
    }

    // 3. ADD
    public override async Task<Entities.LearningStep> AddAsync(Entities.LearningStep entity)
    {
        // Map Entity -> Protobuf
        var protoObj = new via.sep3.dataserver.grpc.LearningStep
        {
            Type = entity.Type,
            Content = entity.Content,
            CourseId = entity.CourseId
            // ID is usually 0/null here, set by DB later
        };

        var response = await Client.AddLearningStepAsync(protoObj);

        // Return the entity with the new ID assigned by the database
        entity.Id = response.Id;
        return entity;
    }

    // 4. UPDATE
    public override async Task UpdateAsync(Entities.LearningStep entity)
    {
        var protoObj = new via.sep3.dataserver.grpc.LearningStep
        {
            Id = entity.Id,
            Type = entity.Type,
            Content = entity.Content,
            CourseId = entity.CourseId
        };

        await Client.UpdateLearningStepAsync(protoObj);
    }

    // 5. DELETE
    public override async Task DeleteAsync(int id)
    {
        await Client.DeleteLearningStepAsync(new IdRequest { Id = id });
    }

    // 6. CLEAR (Optional/Test specific)
    public override Task ClearAsync()
    {
        throw new NotImplementedException("ClearAsync is generally not safe for production DBs");
    }
}