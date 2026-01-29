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
    public decimal AllocatedSpaceMB { get; set; }
    public decimal UsedSpaceMB { get; set; }
    public decimal FreeSpaceMB { get; set; }
    public int UsedPercent { get; set; }
    public decimal MaxSizeMB { get; set; }
    public string AutogrowSize { get; set; } = string.Empty;
    public decimal TotalDriveMB { get; set; }
    public decimal FreeDriveMB { get; set; }
    public decimal FreeDrivePercent { get; set; }
}
