namespace XTCheck.Api.Models;

public class DbSizeStats
{
    public DateTime ExtTime { get; set; }
    public string InstanceName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string LogicalFileName { get; set; } = string.Empty;
    public string FileGroup { get; set; } = string.Empty;
    public string PhysicalFileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long AllocatedSpaceMB { get; set; }
    public long UsedSpaceMB { get; set; }
    public long FreeSpaceMB { get; set; }
    public long UsedPercent { get; set; }
    public long MaxSizeMB { get; set; }
    public string AutogrowSize { get; set; } = string.Empty;
    public long TotalDriveMB { get; set; }
    public long FreeDriveMB { get; set; }
    public long FreeDrivePercent { get; set; }
}
