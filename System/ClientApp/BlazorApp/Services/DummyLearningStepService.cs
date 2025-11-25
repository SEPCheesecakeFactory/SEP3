using BlazorApp.Entities;

namespace BlazorApp.Services;

public class DummyLearningStepService : ILearningStepService
{
    public Task<LearningStep> GetLearningStepAsync(int courseId, int stepOrder)
    {
        return Task.FromResult(new LearningStep
        {
            CourseId = courseId,
            StepOrder = stepOrder,
            StepType = "Video",
            Content = "Dummy content: later replaced with real data."
        });
    }
}
