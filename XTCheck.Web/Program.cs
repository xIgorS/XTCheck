using Radzen;
using XTCheck.Web.Components;
using XTCheck.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Radzen services
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

// Configure HttpClient for API calls
builder.Services.AddHttpClient<IDbSizeStatsApiClient, DbSizeStatsApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5239/");
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

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
