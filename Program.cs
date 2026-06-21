using tubes_kpl.Components;
using tubes_kpl.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<AdminApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration.GetValue<string>("BackendApi:BaseUrl") ?? "http://localhost:8080");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddScoped<AdminApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
