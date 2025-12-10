using System.Net.Http.Json;
using BlazorApp.Entities;
using BlazorApp.Shared;

namespace BlazorApp.Services;

public class HttpLearningStepService(HttpCrudService service) : ILearningStepService
{
    // GET LEARNING STEP
    public async Task<Optional<LearningStep>> GetLearningStepAsync(int courseId, int stepOrder)
{
    try
    {
        var result = await service.GetAsync<LearningStep>($"learningsteps/{courseId}_{stepOrder}");

        if (result.HasError)
        {
            
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage) &&
                result.ErrorMessage.Contains("404"))
            {
                return Optional<LearningStep>.Error(
                    $"Step {stepOrder} was not found for course {courseId}.");
            }

            return Optional<LearningStep>.Error(result.ErrorMessage);
        }

        return result;
    }
    catch (HttpRequestException)
    {
        return Optional<LearningStep>.Error("NO_CONNECTION");
    }
    catch (Exception ex)
    {
        return Optional<LearningStep>.Error("Failed to load learning step: " + ex.Message);
    }
}


    // UPDATE LEARNING STEP
    public async Task<Optional<LearningStep>> UpdateLearningStepAsync(LearningStep updatedStep)
    {
        try
        {
            // return await service.UpdateAsync<LearningStep, LearningStep>($"learningsteps/{updatedStep.CourseId}_{updatedStep.StepOrder}", updatedStep);

            var result = await service.UpdateAsync<LearningStep, LearningStep>(
                $"learningsteps/{updatedStep.CourseId}_{updatedStep.StepOrder}",
                updatedStep
            );

            return result; // Optional<LearningStep>
        }
        catch (Exception ex)
        {
            return Optional<LearningStep>.Error("Failed to update learning step: " + ex.Message);
        }
    }

    // CREATE LEARNING STEP
    public async Task<Optional<LearningStep>> CreateLearningStepAsync(LearningStep newStep)
    {
        try
        {
            // return await service.CreateAsync<LearningStep, LearningStep>("learningsteps", newStep);

            var result = await service.CreateAsync<LearningStep, LearningStep>("learningsteps", newStep);
            return result; // Optional<LearningStep>
        }
        catch (Exception ex)
        {
            return Optional<LearningStep>.Error("Failed to create learning step: " + ex.Message);
        }
    }
}
