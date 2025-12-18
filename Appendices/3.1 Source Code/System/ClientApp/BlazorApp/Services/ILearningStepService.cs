using BlazorApp.Entities;
using BlazorApp.Shared;

namespace BlazorApp.Services;

public interface ILearningStepService
{
    // GET LEARNING STEP
    Task<Optional<LearningStep>> GetLearningStepAsync(int courseId, int stepOrder);

    // UPDATE LEARNING STEP
    Task<Optional<LearningStep>> UpdateLearningStepAsync(LearningStep updatedStep);
    // CREATE LEARNING STEP
    Task<Optional<LearningStep>> CreateLearningStepAsync(LearningStep newStep);
}
