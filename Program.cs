using tubes_kpl.Components;
using tubes_kpl.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();

var backendUrl = builder.Configuration["BackendUrl"] ?? throw new InvalidOperationException("BackendUrl is not configured.");

builder.Services.AddHttpClient("BackendAPI", client =>
{
    client.BaseAddress = new Uri(backendUrl);
});

// Register Services
builder.Services.AddScoped<CampaignService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();