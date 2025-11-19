
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
            Id = ls.Id,
            Type = ls.Type,
            Content = ls.Content,
            CourseId = ls.CourseId
        });
    }

    // GET /LearningStep?courseId=...
    [HttpGet]
    public ActionResult<IEnumerable<learningStepDto>> GetAllLearningSteps(
        [FromQuery] int? courseId)
    {
        var learningSteps = learningStepsRepository.GetMany();

        // Filter by courseId text
        if (courseId.HasValue)
        {
            learningSteps = learningSteps.Where(ls =>
                ls.CourseId.HasValue &&
                ls.CourseId.Value == courseId.Value);
        }

        var learningStepDtos = learningSteps
            .Select(ls => new learningStepDto
            {
                Id = ls.Id,
                Type = ls.Type,
                Content = ls.Content,
                CourseId = ls.CourseId
            })
            .ToList();

        return Ok(learningStepDtos);
    }
}