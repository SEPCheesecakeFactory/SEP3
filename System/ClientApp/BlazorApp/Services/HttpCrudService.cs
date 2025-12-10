using System;
using System.Text.Json;
using BlazorApp.Shared;

namespace BlazorApp.Services;

public class HttpCrudService(HttpClient client)
{
    private readonly HttpClient client = client;
    private readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // CREATE
    public async Task<Optional<T>> CreateAsync<T, R>(string endpoint, R request)
    {
        try
        {
            HttpResponseMessage httpResponse = await client.PostAsJsonAsync(endpoint, request);
            string response = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
                return Optional<T>.Error(response);

            var value = JsonSerializer.Deserialize<T>(response, jsonOptions);
            return Optional<T>.Success(value!);
        }
        catch (Exception ex)
        {
            return Optional<T>.Error("Create failed: " + ex.Message);
        }
    }

    // GET
    public async Task<Optional<T>> GetAsync<T>(string endpoint, int? id = null)
    {
        try
        {
            var finalEndpoint = id.HasValue ? $"{endpoint}/{id.Value}" : endpoint;
            HttpResponseMessage httpResponse = await client.GetAsync(finalEndpoint);
            string response = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
                return Optional<T>.Error(response);

            var value = JsonSerializer.Deserialize<T>(response, jsonOptions);
            return Optional<T>.Success(value!);
        }
        catch (Exception ex)
        {
            return Optional<T>.Error("Get failed: " + ex.Message);
        }
    }

    // DELETE
    public async Task<Optional<bool>> DeleteAsync(string endpoint, int? id = null)
    {
        try
        {
            var finalEndpoint = id.HasValue ? $"{endpoint}/{id.Value}" : endpoint;
            HttpResponseMessage httpResponse = await client.DeleteAsync(finalEndpoint);
            string response = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
                return Optional<bool>.Error(response);

            return Optional<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Optional<bool>.Error("Delete failed: " + ex.Message);
        }
    }

    // UPDATE
    public async Task<Optional<T>> UpdateAsync<T, R>(string endpoint, R request, int? id = null)
    {
        try
        {
            var finalEndpoint = id.HasValue ? $"{endpoint}/{id.Value}" : endpoint;
            HttpResponseMessage httpResponse = await client.PutAsJsonAsync(finalEndpoint, request);
            string response = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
                return Optional<T>.Error(response);

            var value = JsonSerializer.Deserialize<T>(response, jsonOptions);
            return Optional<T>.Success(value!);
        }
        catch (Exception ex)
        {
            return Optional<T>.Error("Update failed: " + ex.Message);
        }
    }
}
