using System.Net.Http.Json;
using BlazorApp.Entities;

namespace BlazorApp.Services;

public class LearningStepHttpService : ILearningStepService
{
    private readonly HttpClient client;

    public LearningStepHttpService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<LearningStep> GetLearningStepAsync(int courseId, int stepOrder)
    {
        var response = await client.GetFromJsonAsync<LearningStep>(
            $"learningsteps/{courseId}_{stepOrder}");

        if (response is null)
            throw new Exception("Learning step not found");

        return response;
    }
}
