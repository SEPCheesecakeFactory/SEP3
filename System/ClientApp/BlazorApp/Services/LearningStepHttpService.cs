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
        LearningStep? response = null;

        try
        {
            response = await client.GetFromJsonAsync<LearningStep>(
                $"learningsteps/{courseId}_{stepOrder}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Handle 404 Not Found specifically if needed
            throw new Exception("Learning step not found", ex);
        }

        if (response is null)
            throw new Exception("Learning step not found");

        return response;
    }
}
