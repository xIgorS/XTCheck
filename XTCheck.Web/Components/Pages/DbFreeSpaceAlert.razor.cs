using Microsoft.AspNetCore.Components;
using ApexCharts;
using XTCheck.Web.Models;
using XTCheck.Web.Services;

namespace XTCheck.Web.Components.Pages;

public partial class DbFreeSpaceAlert
{
    [Inject] protected IDbSizeStatsApiClient ApiClient { get; set; } = null!;

    private List<DbSizeAlertStats> alerts = new();
    private bool isLoading = true;
    private string? errorMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadAlerts();
    }

    private async Task LoadAlerts()
    {
        isLoading = true;
        errorMessage = null;

        try
        {
            var result = await ApiClient.GetDbFreeSpaceAlertsAsync();
            alerts = result.ToList();
        }
        catch (Exception ex)
        {
            errorMessage = $"Failed to load alerts: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }

    private string NormalizeSeverity(string? alertLevel)
    {
        if (string.IsNullOrWhiteSpace(alertLevel))
        {
            return "Unspecified";
        }

        if (alertLevel.Equals("critical", StringComparison.OrdinalIgnoreCase))
        {
            return "Critical";
        }

        if (alertLevel.Equals("warning", StringComparison.OrdinalIgnoreCase))
        {
            return "Warning";
        }

        if (alertLevel.Equals("ok", StringComparison.OrdinalIgnoreCase))
        {
            return "OK";
        }

        return alertLevel;
    }

    private string GetAlertLabel(DbSizeAlertStats alert)
        => alert.FileGroup;

    private Dictionary<string, List<DbSizeAlertStats>> GetAlertsByDatabase()
        => alerts.GroupBy(a => a.DatabaseName ?? "Unknown")
                 .ToDictionary(g => g.Key, g => g.ToList());

    private int GetChartHeight(int itemCount)
        => Math.Max(100, itemCount * 25 + 50);

    private ApexChartOptions<DbSizeAlertStats> GetChartOptions(string dbName)
    {
        return new ApexChartOptions<DbSizeAlertStats>
        {
            Chart = new Chart { Type = ChartType.Bar, Stacked = true, Id = $"chart-{dbName}" },
            Colors = new List<string> { "#F44336", "#4CAF50" },
            PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar { Horizontal = true, BarHeight = "70%" }
            },
            Legend = new Legend { Position = ApexCharts.LegendPosition.Top },
            Xaxis = new XAxis 
            { 
                Labels = new XAxisLabels
                {
                    Style = new AxisLabelStyle
                    {
                        FontSize = "12px",
                        FontWeight = "500"
                    }
                }
            },
            Yaxis = new List<YAxis>
            {
                new()
                {
                    Title = new AxisTitle { Text = "Size (MB)" },
                    Labels = new YAxisLabels
                    {
                        Style = new AxisLabelStyle
                        {
                            FontSize = "13px",
                            FontWeight = "600"
                        }
                    }
                }
            }
        };
    }

    private string GetSeverityBadgeClass(string alertLevel)
    {
        var severity = NormalizeSeverity(alertLevel);
        return severity switch
        {
            "Critical" => "badge-pill status-critical",
            "Warning" => "badge-pill status-warning",
            "OK" => "badge-pill status-ok",
            _ => "badge-pill status-neutral"
        };
    }

    private string GetSeverityCellClass(string alertLevel)
    {
        var severity = NormalizeSeverity(alertLevel);
        return severity switch
        {
            "Critical" => "severity-cell severity-critical",
            "Warning" => "severity-cell severity-warning",
            "OK" => "severity-cell severity-ok",
            _ => "severity-cell"
        };
    }
}