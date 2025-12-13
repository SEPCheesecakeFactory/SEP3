using BlazorApp.Entities;
using BlazorApp.Shared;

namespace BlazorApp.Services;

public class DummyLearningStepService : ILearningStepService
{
    public Task<Optional<LearningStep>> GetLearningStepAsync(int courseId, int stepOrder)
    {
        var step = new LearningStep
        {
            CourseId = courseId,
            StepOrder = stepOrder,
            Type = "Video",
            Content = "Dummy content: later replaced with real data."
        };

        return Task.FromResult(Optional<LearningStep>.Success(step));
    }

    public Task<Optional<LearningStep>> UpdateLearningStepAsync(LearningStep updatedStep)
    {
        return Task.FromResult(Optional<LearningStep>.Success(updatedStep));
    }

    public Task<Optional<LearningStep>> CreateLearningStepAsync(LearningStep newStep)
    {
        return Task.FromResult(Optional<LearningStep>.Success(newStep));
    }
}
