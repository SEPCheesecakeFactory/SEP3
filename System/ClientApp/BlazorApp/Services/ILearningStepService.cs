using BlazorApp.Entities;
namespace BlazorApp.Services;

public interface ILearningStepService
{
    Task<LearningStep> GetLearningStepAsync(int courseId, int stepOrder);
    Task<LearningStep> UpdateLearningStepAsync(LearningStep updatedStep);
    Task<LearningStep> CreateLearningStepAsync(LearningStep newStep);
}
