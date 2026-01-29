using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authentication.Negotiate;
using XTCheck.Api.Data;
using XTCheck.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IDbSizeStatsRepository, DbSizeStatsRepository>();
builder.Services.AddScoped<IDbSizeStatsService, DbSizeStatsService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Determine if running on Windows
bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // Power BI Desktop uses Power Query (native HTTP client), not a browser,
        // so CORS is not strictly required. This permissive policy ensures flexibility.
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Allowed Windows users (case-insensitive comparison)
var allowedUsers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{
    @"domain1\user1",
    @"domain2\user2"
};

// Conditional Authentication based on OS
if (isWindows)
{
    // Windows: Use Windows Authentication (Negotiate/NTLM/Kerberos)
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        .AddNegotiate();

    builder.Services.AddAuthorization(options =>
    {
        // Create a policy that requires the user to be in the allowed list
        options.AddPolicy("AllowedUsersOnly", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
            {
                var userName = context.User.Identity?.Name;
                return !string.IsNullOrEmpty(userName) && allowedUsers.Contains(userName);
            });
        });

        // Apply this policy to all endpoints by default
        options.FallbackPolicy = options.GetPolicy("AllowedUsersOnly");
    });
}
else
{
    // macOS/Linux: No authentication (anonymous access)
    builder.Services.AddAuthorization();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS must come before Authentication
app.UseCors();

if (isWindows)
{
    app.UseAuthentication();
}
app.UseAuthorization();

app.MapControllers();

app.Run();
