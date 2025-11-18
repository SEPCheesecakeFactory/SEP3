using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using ApiContracts;

namespace RESTAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class LearningStepController(IRepository<learningStep> learningStepsRepository) : ControllerBase
{
    private readonly IRepository<learningStep> _learningStepsRepository = learningStepsRepository;

    [HttpGet("{id}")]
    public async Task<ActionResult<learningStepDto>> GetLearningStep(int id)
    {
        var step = await _learningStepsRepository.GetSingleAsync(id);
        
        if (step == null)
            return NotFound();

        // Map to DTO using the new properties
        return Ok(new learningStepDto 
        { 
            Id = step.Id, 
            Question = step.Question, 
            Answer = step.Answer 
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<learningStepDto>>> GetAllLearningSteps(
        [FromQuery] string? search)
    {
        // Get all items
        // Note: If the generic repository returns IQueryable, the filtering happens in the DB. 
        // If it returns IEnumerable, it happens in memory.
        var query = _learningStepsRepository.GetMany().AsQueryable();

        // Apply Text Search Filter (Replacing the old User/Course filters)
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => 
                (x.Question != null && x.Question.Contains(search, StringComparison.OrdinalIgnoreCase)) || 
                (x.Answer != null && x.Answer.Contains(search, StringComparison.OrdinalIgnoreCase))
            );
        }

        // Map to DTO
        var learningStepDtos = query.Select(step => new learningStepDto
        {
            Id = step.Id,
            Question = step.Question,
            Answer = step.Answer
        }).ToList();

        return Ok(learningStepDtos);
    }
}