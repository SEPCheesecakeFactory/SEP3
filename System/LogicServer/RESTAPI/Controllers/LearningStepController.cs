using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using RESTAPI.Dtos; // namespace for DTOs
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RESTAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearningStepController : ControllerBase
{
    private readonly IRepositoryID<LearningStep, (int, int)> _repository;

    public LearningStepController(IRepositoryID<LearningStep, (int, int)> repository)
    {
        _repository = repository;
    }

    // GET: api/learningstep/course/1
    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<IEnumerable<LearningStepDto>>> GetByCourse(int courseId)
    {
        var steps = _repository.GetMany().Where(s => s.CourseId == courseId);
        
        var dtos = steps.Select(s => new LearningStepDto
        {
            CourseId = s.CourseId,
            // Map StepOrder as the "Id" for the DTO if the frontend expects a single ID
            // OR keep them separate if the DTO supports it
            Id = s.StepOrder, 
            Type = s.Type,
            Content = s.Content,
            // Add StepOrder to DTO if needed
        });

        return Ok(dtos);
    }

    // GET: api/learningstep/1/2 (Course 1, Step 2)
    [HttpGet("{courseId}/{stepOrder}")]
    public async Task<ActionResult<LearningStepDto>> GetSingle(int courseId, int stepOrder)
    {
        try 
        {
            var s = await _repository.GetSingleAsync((courseId, stepOrder));
            return Ok(new LearningStepDto 
            { 
                CourseId = s.CourseId, 
                Id = s.StepOrder, 
                Type = s.Type, 
                Content = s.Content 
            });
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<LearningStepDto>> Add([FromBody] CreateLearningStepDto dto)
    {
        var entity = new LearningStep
        {
            CourseId = dto.CourseId,
            Type = dto.Type,
            Content = dto.Content
        };

        var created = await _repository.AddAsync(entity);

        return Ok(new LearningStepDto
        {
            CourseId = created.CourseId,
            Id = created.StepOrder,
            Type = created.Type,
            Content = created.Content
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] LearningStepDto dto)
    {
        var entity = new LearningStep
        {
            CourseId = dto.CourseId,
            StepOrder = dto.Id, // Assuming Id holds the order
            Type = dto.Type,
            Content = dto.Content
        };

        await _repository.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{courseId}/{stepOrder}")]
    public async Task<IActionResult> Delete(int courseId, int stepOrder)
    {
        await _repository.DeleteAsync((courseId, stepOrder));
        return NoContent();
    }
}