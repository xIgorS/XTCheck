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

// No authentication - allow anonymous access
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS must come before Authentication
app.UseCors();

// No authentication middleware
app.MapControllers();

app.Run();
