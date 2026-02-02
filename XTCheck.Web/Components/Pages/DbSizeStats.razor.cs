using Microsoft.AspNetCore.Components;
using ApexCharts;
using XTCheck.Web.Models;
using XTCheck.Web.Services;

namespace XTCheck.Web.Components.Pages;

public partial class DbSizeStats
{
    [Inject] protected IDbSizeStatsApiClient ApiClient { get; set; } = null!;

    private List<DatabaseSizeAggregate> databases = new();
    private List<FileGroupAggregate> fileGroups = new();
    private List<DiskSpaceInfo> diskSpaceInfo = new();
    private string? selectedDatabase;
    private DatabaseSizeAggregate? selectedDatabaseData;
    private List<ChartDataItem> dbSpaceChartData = new();
    private List<ChartDataItem> diskChartData = new();
    private bool isLoading = true;
    private string? errorMessage;

    // ApexCharts Options
    private ApexChartOptions<ChartDataItem> dbSpaceChartOptions = new()
    {
        Chart = new Chart { Type = ChartType.Donut },
        Colors = new List<string> { "#4CAF50", "#F44336" },
        Legend = new Legend { Position = ApexCharts.LegendPosition.Bottom },
        PlotOptions = new PlotOptions
        {
            Pie = new PlotOptionsPie
            {
                Donut = new PlotOptionsDonut
                {
                    Labels = new DonutLabels
                    {
                        Show = true,
                        Total = new DonutLabelTotal
                        {
                            Show = true,
                            Label = "Total",
                            FontSize = "16px"
                        }
                    }
                }
            }
        },
        DataLabels = new DataLabels
        {
            Enabled = true,
            Formatter = "function(val, opts) { return opts.w.config.series[opts.seriesIndex].toFixed(1) + ' MB'; }"
        }
    };

    private ApexChartOptions<DatabaseSizeAggregate> allDbChartOptions = new()
    {
        Chart = new Chart { Type = ChartType.Bar, Stacked = true },
        Colors = new List<string> { "#F44336", "#4CAF50" },
        PlotOptions = new PlotOptions
        {
            Bar = new PlotOptionsBar { Horizontal = true }
        },
        Xaxis = new XAxis { Title = new AxisTitle { Text = "Size (MB)" } },
        Legend = new Legend { Position = ApexCharts.LegendPosition.Top }
    };

    private ApexChartOptions<FileGroupAggregate> fileGroupChartOptions = new()
    {
        Chart = new Chart { Type = ChartType.Bar, Stacked = true },
        Colors = new List<string> { "#F44336", "#4CAF50" },
        PlotOptions = new PlotOptions
        {
            Bar = new PlotOptionsBar { Horizontal = false }
        },
        Xaxis = new XAxis { Title = new AxisTitle { Text = "FileGroup" } },
        Yaxis = new List<YAxis> { new YAxis { Title = new AxisTitle { Text = "Size (MB)" } } },
        Legend = new Legend { Position = ApexCharts.LegendPosition.Bottom }
    };

    private ApexChartOptions<DiskSpaceInfo> diskRadialOptions = new()
    {
        Chart = new Chart { Type = ChartType.RadialBar },
        Colors = new List<string> { "#2196F3" },
        PlotOptions = new PlotOptions
        {
            RadialBar = new PlotOptionsRadialBar
            {
                Hollow = new Hollow { Size = "70%" },
                DataLabels = new RadialBarDataLabels
                {
                    Show = true,
                    Name = new RadialBarDataLabelsName { Show = true, FontSize = "16px" },
                    Value = new RadialBarDataLabelsValue 
                    { 
                        Show = true, 
                        FontSize = "24px",
                        Formatter = "function(val) { return val + '% Used'; }"
                    }
                }
            }
        }
    };

    private ApexChartOptions<ChartDataItem> diskDonutOptions = new()
    {
        Chart = new Chart { Type = ChartType.Donut },
        Colors = new List<string> { "#F44336", "#4CAF50" },
        Legend = new Legend { Position = ApexCharts.LegendPosition.Bottom }
    };

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        errorMessage = null;
        StateHasChanged();

        try
        {
            var result = await ApiClient.GetAggregatedDbSizeStatsAsync();
            databases = result.ToList();

            // Load disk space info
            var diskResult = await ApiClient.GetDiskSpaceInfoAsync();
            diskSpaceInfo = diskResult.ToList();
            UpdateDiskChartData();

            if (databases.Any())
            {
                selectedDatabase ??= databases.First().DatabaseName;
                await UpdateSelectedDatabaseData();
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Failed to load data: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task OnDatabaseSelected()
    {
        await UpdateSelectedDatabaseData();
    }

    private async Task UpdateSelectedDatabaseData()
    {
        selectedDatabaseData = databases.FirstOrDefault(d => d.DatabaseName == selectedDatabase);
        
        if (selectedDatabaseData != null)
        {
            dbSpaceChartData = new List<ChartDataItem>
            {
                new ChartDataItem { Label = "Used", Value = selectedDatabaseData.TotalUsedMB },
                new ChartDataItem { Label = "Free", Value = selectedDatabaseData.TotalFreeMB }
            };

            // Load FileGroup stats for selected database
            var fgResult = await ApiClient.GetFileGroupStatsAsync(selectedDatabase!);
            fileGroups = fgResult.ToList();
        }
    }

    private void UpdateDiskChartData()
    {
        if (diskSpaceInfo.Any())
        {
            // Aggregate all disk space for the chart
            var totalUsed = diskSpaceInfo.Sum(d => d.UsedDriveMB);
            var totalFree = diskSpaceInfo.Sum(d => d.FreeDriveMB);
            
            diskChartData = new List<ChartDataItem>
            {
                new ChartDataItem { Label = "Used", Value = totalUsed },
                new ChartDataItem { Label = "Free", Value = totalFree }
            };
        }
    }

    // Chart data model for ApexCharts
    public class ChartDataItem
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }
}