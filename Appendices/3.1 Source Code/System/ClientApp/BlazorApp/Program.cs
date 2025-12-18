using BlazorApp.Auth;
using BlazorApp.Components;
using BlazorApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

// ==================================
//   App builder
// ==================================

var builder = WebApplication.CreateBuilder(args);

// ==================================
//   Razor Components
// ==================================

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ==================================
//   HTTP Client Configuration
// ==================================

var hostUri = builder.Configuration["HostUri"] ?? "http://localhost:5161";

builder.Services.AddHttpClient("LocalApi", client =>
{
    client.BaseAddress = new Uri(hostUri);
});

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("LocalApi"));

// ==================================
//   Services
// ==================================

// Domain Services
builder.Services.AddScoped<HttpCrudService>();
builder.Services.AddScoped<ICourseService, HttpCourseService>();
builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<ILearningStepService, HttpLearningStepService>();

// Auth Services
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();

// ==================================
//   Authentication & Authorization
// ==================================

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Prevents XSS scripts from reading the cookie
        options.Cookie.HttpOnly = true;
        // Ensures cookie is sent only over HTTPS
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        // Prevents CSRF attacks
        options.Cookie.SameSite = SameSiteMode.Strict;
        // UX: Keeps user logged in as long as they are active
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddAuthorization();

// ==================================
//   Antiforgery
// ==================================

builder.Services.AddAntiforgery();

// ==================================
//   Build app
// ==================================

var app = builder.Build();

// ==================================
//   Middleware
// ==================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Maps static files
app.MapStaticAssets();

// Security Middleware (Order matters here!)
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();