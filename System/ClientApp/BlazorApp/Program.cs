using BlazorApp.Components;
using BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var hostUri = builder.Configuration["HostUri"] ?? "http://localhost:5161";

builder.Services.AddHttpClient<ICourseService, HttpCourseService>(c => c.BaseAddress = new Uri(hostUri));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
