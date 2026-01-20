namespace XTCheck.Api.Models;

public class DbSizeStats
{
    public string DatabaseName { get; set; } = string.Empty;
    public string LogicalFileName { get; set; } = string.Empty;
    public string FileGroup { get; set; } = string.Empty;
    public string PhysicalFileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public decimal DatabaseFileSizeMB { get; set; }
    public string MaxSize { get; set; } = string.Empty;
    public string AutogrowSize { get; set; } = string.Empty;
    public string Drive { get; set; } = string.Empty;
    public long TotalDriveGB { get; set; }
    public long FreeDriveGB { get; set; }
    public decimal FreePercent { get; set; }
}
