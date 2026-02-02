using Radzen;
using XTCheck.Web.Components;
using XTCheck.Web.Services;
using Microsoft.AspNetCore.Authentication.Negotiate;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Determine if running on Windows
bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

// Add Windows Authentication to Web project
if (isWindows)
{
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        .AddNegotiate();
    builder.Services.AddAuthorization();
}

// Add Radzen services
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

// Configure HttpClient for API calls
builder.Services.AddHttpClient<IDbSizeStatsApiClient, DbSizeStatsApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7160/");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Use default Windows credentials when running on Windows
    UseDefaultCredentials = true,
    // Allow automatic redirection
    AllowAutoRedirect = true,
    // Set credential cache to use default network credentials
    Credentials = System.Net.CredentialCache.DefaultNetworkCredentials
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Add authentication middleware for Windows
if (isWindows)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
