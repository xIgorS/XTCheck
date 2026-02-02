using Microsoft.Data.SqlClient;

namespace XTCheck.Api.Data;

public interface IDbInitializer
{
    Task InitializeAsync();
}

public class DbInitializer : IDbInitializer
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(IConfiguration configuration, ILogger<DbInitializer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Extract the database name and create a connection to master for initial setup
        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;
        builder.InitialCatalog = "master";
        var masterConnectionString = builder.ConnectionString;

        try
        {
            // Step 1: Ensure the database exists
            await EnsureDatabaseExistsAsync(masterConnectionString, databaseName);

            // Step 2: Ensure schema and objects exist
            await EnsureSchemaExistsAsync(connectionString);

            // Step 3: Run table scripts
            await RunTableScriptsAsync(connectionString);

            // Step 4: Run stored procedure scripts
            await RunStoredProcedureScriptsAsync(connectionString);

            _logger.LogInformation("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize database");
            throw;
        }
    }

    private async Task EnsureDatabaseExistsAsync(string masterConnectionString, string databaseName)
    {
        await using var connection = new SqlConnection(masterConnectionString);
        await connection.OpenAsync();

        var checkDbSql = $@"
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = @DatabaseName)
            BEGIN
                CREATE DATABASE [{databaseName}]
            END";

        using var command = new SqlCommand(checkDbSql, connection);
        command.Parameters.AddWithValue("@DatabaseName", databaseName);
        await command.ExecuteNonQueryAsync();

        _logger.LogInformation("Ensured database '{DatabaseName}' exists", databaseName);
    }

    private async Task EnsureSchemaExistsAsync(string connectionString)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var checkSchemaSql = @"
            IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'monitoring')
            BEGIN
                EXEC('CREATE SCHEMA [monitoring]')
            END";

        using var command = new SqlCommand(checkSchemaSql, connection);
        await command.ExecuteNonQueryAsync();

        _logger.LogInformation("Ensured schema 'monitoring' exists");
    }

    private async Task RunTableScriptsAsync(string connectionString)
    {
        var tablesPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Database", "Tables");
        
        // Fallback to relative path from content root if running from source
        if (!Directory.Exists(tablesPath))
        {
            tablesPath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "Tables");
        }

        if (!Directory.Exists(tablesPath))
        {
            _logger.LogWarning("Tables directory not found at {Path}. Skipping table initialization.", tablesPath);
            return;
        }

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        foreach (var file in Directory.GetFiles(tablesPath, "*.sql"))
        {
            var scriptContent = await File.ReadAllTextAsync(file);
            
            // Remove USE statements and split by GO
            scriptContent = RemoveUseStatements(scriptContent);
            var batches = SplitByGo(scriptContent);

            foreach (var batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch)) continue;

                try
                {
                    using var command = new SqlCommand(batch, connection);
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error executing batch from {File}: {Batch}", Path.GetFileName(file), batch[..Math.Min(100, batch.Length)]);
                }
            }

            _logger.LogInformation("Executed table script: {FileName}", Path.GetFileName(file));
        }
    }

    private async Task RunStoredProcedureScriptsAsync(string connectionString)
    {
        var spPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Database", "StoredProcedures");
        
        // Fallback to relative path from content root if running from source
        if (!Directory.Exists(spPath))
        {
            spPath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "StoredProcedures");
        }

        if (!Directory.Exists(spPath))
        {
            _logger.LogWarning("StoredProcedures directory not found at {Path}. Skipping SP initialization.", spPath);
            return;
        }

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        foreach (var file in Directory.GetFiles(spPath, "*.sql"))
        {
            var scriptContent = await File.ReadAllTextAsync(file);
            
            // Remove USE statements and split by GO
            scriptContent = RemoveUseStatements(scriptContent);
            var batches = SplitByGo(scriptContent);

            foreach (var batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch)) continue;

                try
                {
                    using var command = new SqlCommand(batch, connection);
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error executing batch from {File}: {Batch}", Path.GetFileName(file), batch[..Math.Min(100, batch.Length)]);
                }
            }

            _logger.LogInformation("Executed stored procedure script: {FileName}", Path.GetFileName(file));
        }
    }

    private static string RemoveUseStatements(string script)
    {
        // Remove USE [DatabaseName] statements since we're already connected to the right DB
        var lines = script.Split('\n');
        var filteredLines = lines.Where(line => 
            !line.Trim().StartsWith("USE ", StringComparison.OrdinalIgnoreCase) &&
            !line.Trim().StartsWith("USE[", StringComparison.OrdinalIgnoreCase));
        return string.Join('\n', filteredLines);
    }

    private static IEnumerable<string> SplitByGo(string script)
    {
        // Split script by GO statements (case insensitive, whole word)
        return System.Text.RegularExpressions.Regex.Split(script, @"^\s*GO\s*$", 
            System.Text.RegularExpressions.RegexOptions.Multiline | 
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            .Where(batch => !string.IsNullOrWhiteSpace(batch));
    }
}
