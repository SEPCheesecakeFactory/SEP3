using System;
using System.Security.Claims;
using System.Text.Json;
using BlazorApp.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp.Auth;

public class SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime) : AuthenticationStateProvider
{
    private readonly HttpClient httpClient = httpClient;
    private readonly ClaimsPrincipal currentClaimsPrincipal;
    private readonly IJSRuntime jsRuntime = jsRuntime;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task LoginASync(string userName, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login",
            new LoginRequest { Username = userName, Password = password });

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, jsonSerializerOptions)!;

        string serialisedData = JsonSerializer.Serialize(userDto);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);

        List<Claim> claims =
        [
            new(ClaimTypes.Name, userDto.Username),
            new(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
        ];

        ClaimsIdentity identity = new(claims, "apiauth");
        ClaimsPrincipal claimsPrincipal = new(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(claimsPrincipal))
        );
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string userAsJson = "";
        try
        {
            userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        }
        catch (InvalidOperationException e)
        {
            return new AuthenticationState(new());
        }

        if (string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new());
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(userAsJson)!;
        List<Claim> claims =
        [
            new(ClaimTypes.Name, userDto.Username),
            new(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
        ];
        ClaimsIdentity identity = new(claims, "apiauth");
        ClaimsPrincipal claimsPrincipal = new(identity);
        return new AuthenticationState(claimsPrincipal);
    }

    public async Task Logout()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }
}


