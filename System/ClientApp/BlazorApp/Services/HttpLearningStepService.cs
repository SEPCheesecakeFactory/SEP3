using System.Net.Http.Json;
using BlazorApp.Entities;

namespace BlazorApp.Services;

public class HttpLearningStepService(HttpCrudService service) : ILearningStepService
{
    public async Task<LearningStep> GetLearningStepAsync(int courseId, int stepOrder) => await service.GetAsync<LearningStep>($"learningsteps/{courseId}_{stepOrder}");
    public Task<LearningStep> UpdateLearningStepAsync(LearningStep updatedStep) => service.UpdateAsync<LearningStep, LearningStep>($"learningsteps/{updatedStep.CourseId}_{updatedStep.StepOrder}", updatedStep);
}
