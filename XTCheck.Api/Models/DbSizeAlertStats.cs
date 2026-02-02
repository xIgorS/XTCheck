namespace XTCheck.Api.Models;

public class DbSizeAlertStats
{
    public string DatabaseName { get; set; } = string.Empty;
    public string FileGroup { get; set; } = string.Empty;
    public long AllocatedDBSpaceMB { get; set; }
    public long UsedDBSpaceMB { get; set; }
    public long FreeDBSpaceMB { get; set; }
    public bool AutogrowEnabled { get; set; }
    public long FreeDriveMB { get; set; }
    public long PartSizeMB { get; set; }
    public long TotalFreeSpaceMB { get; set; }
    public string AlertLevel { get; set; } = string.Empty;
}
