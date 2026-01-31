namespace XTCheck.Web.Models;

public class DbSizeAlertStats
{
    public string DatabaseName { get; set; } = string.Empty;
    public string FileGroup { get; set; } = string.Empty;
    public int AllocatedDBSpaceMB { get; set; }
    public decimal UsedDBSpaceMB { get; set; }
    public decimal FreeDBSpaceMB { get; set; }
    public bool AutogrowEnabled { get; set; }
    public decimal FreeDriveMB { get; set; }
    public int PartSizeMB { get; set; }
    public decimal TotalFreeSpaceMB { get; set; }
    public string AlertLevel { get; set; } = string.Empty;
}
