using BlazorApp.Components;
using BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var hostUri = builder.Configuration["HostUri"] ?? "http://localhost:5161";

// Existing service
builder.Services.AddHttpClient<ICourseService, HttpCourseService>(c =>
    c.BaseAddress = new Uri(hostUri));

// NEW: Dummy service for now (swap when API is ready)
builder.Services.AddScoped<ILearningStepService, DummyLearningStepService>();

// If REST API becomes available, replace with:
// builder.Services.AddHttpClient<ILearningStepService, LearningStepHttpService>(c =>
//     c.BaseAddress = new Uri(hostUri));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
