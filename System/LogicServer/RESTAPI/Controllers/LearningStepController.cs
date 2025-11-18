
namespace RESTAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class LearningStepController(IRepository learningStepsRepository) : ControllerBase
{
    private readonly IRepository learningStepsRepository = learningStepsRepository;

    // GET /LearningStep/5
    [HttpGet("{id}")]
    public async Task<ActionResult<learningStepDto>> GetLearningStep(int id)
    {
        var learningStep = await learningStepsRepository.GetSingleAsync(id);
        if (learningStep == null)
            return NotFound();

        return Ok(new learningStepDto
        {
            Id = learningStep.Id,
            Question = learningStep.Question,
            Answer = learningStep.Answer
        });
    }

    // GET /LearningStep?question=...&answer=...
    [HttpGet]
    public ActionResult<IEnumerable<learningStepDto>> GetAllLearningSteps(
        [FromQuery] string? question,
        [FromQuery] string? answer)
    {
        var learningSteps = learningStepsRepository.GetMany();

        // Filter by question text
        if (!string.IsNullOrWhiteSpace(question))
        {
            learningSteps = learningSteps.Where(ls =>
                ls.Question != null &&
                ls.Question.Contains(question, StringComparison.OrdinalIgnoreCase));
        }

        // Filter by answer text
        if (!string.IsNullOrWhiteSpace(answer))
        {
            learningSteps = learningSteps.Where(ls =>
                ls.Answer != null &&
                ls.Answer.Contains(answer, StringComparison.OrdinalIgnoreCase));
        }

        var learningStepDtos = learningSteps
            .Select(ls => new learningStepDto
            {
                Id = ls.Id,
                Question = ls.Question,
                Answer = ls.Answer
            })
            .ToList();

        return Ok(learningStepDtos);
    }
}