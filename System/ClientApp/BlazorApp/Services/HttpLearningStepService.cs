using System.Net.Http.Json;
using BlazorApp.Entities;
using BlazorApp.Shared;

namespace BlazorApp.Services;

public class HttpLearningStepService(HttpCrudService service) : ILearningStepService
{
    // GET LEARNING STEP
    public async Task<Optional<LearningStep>> GetLearningStepAsync(int courseId, int stepOrder)
        => await service.GetAsync<LearningStep>(
                $"learningsteps/{courseId}_{stepOrder}"
            );

    // UPDATE LEARNING STEP
    public async Task<Optional<LearningStep>> UpdateLearningStepAsync(LearningStep updatedStep)
        => await service.UpdateAsync<LearningStep, LearningStep>(
                $"learningsteps/{updatedStep.CourseId}_{updatedStep.StepOrder}",
                updatedStep
            );

    // CREATE LEARNING STEP
    public async Task<Optional<LearningStep>> CreateLearningStepAsync(LearningStep newStep)
        => await service.CreateAsync<LearningStep, LearningStep>(
            "learningsteps",
            newStep
        );
}
