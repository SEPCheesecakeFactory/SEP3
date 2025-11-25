using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using RESTAPI.Dtos; // Import the DTOs
using System.Linq;

namespace WebAPI.Controllers;

public class LearningStepController : BaseGrpcController<LearningStep, LearningStepDto, CreateLearningStepDto>
{
    // Inject the repository and pass it to the base
    public LearningStepController(IRepository<LearningStep> repository) : base(repository)
    {
    }

    // --- Implement Mapping Logic ---

    protected override LearningStepDto MapToDto(LearningStep entity)
    {
        return new LearningStepDto
        {
            Id = entity.Id,
            Type = entity.Type,
            Content = entity.Content,
            CourseId = entity.CourseId
        };
    }

    protected override IEnumerable<LearningStepDto> MapToDto(IEnumerable<LearningStep> entities)
    {
        return entities.Select(MapToDto);
    }

    protected override LearningStep MapToEntity(CreateLearningStepDto dto)
    {
        return new LearningStep
        {
            Type = dto.Type,
            Content = dto.Content,
            CourseId = dto.CourseId
        };
    }
}