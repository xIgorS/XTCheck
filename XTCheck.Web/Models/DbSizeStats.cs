namespace XTCheck.Web.Models;

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

/// <summary>
/// Aggregated database size statistics (summed across all files)
/// </summary>
public class DatabaseSizeAggregate
{
    public string DatabaseName { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
    public decimal TotalAllocatedMB { get; set; }
    public decimal TotalUsedMB { get; set; }
    public decimal TotalFreeMB { get; set; }
    public int UsedPercent => TotalAllocatedMB > 0 
        ? (int)Math.Round(TotalUsedMB / TotalAllocatedMB * 100) 
        : 0;
}

/// <summary>
/// Aggregated statistics per FileGroup within a database
/// </summary>
public class FileGroupAggregate
{
    public string DatabaseName { get; set; } = string.Empty;
    public string FileGroup { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public decimal AllocatedMB { get; set; }
    public decimal UsedMB { get; set; }
    public decimal FreeMB { get; set; }
    public int FileCount { get; set; }
    public int UsedPercent => AllocatedMB > 0 
        ? (int)Math.Round(UsedMB / AllocatedMB * 100) 
        : 0;
}

/// <summary>
/// Disk space information aggregated by drive
/// </summary>
public class DiskSpaceInfo
{
    public string DrivePath { get; set; } = string.Empty;
    public decimal TotalDriveMB { get; set; }
    public decimal FreeDriveMB { get; set; }
    public decimal UsedDriveMB => TotalDriveMB - FreeDriveMB;
    public decimal FreeDrivePercent { get; set; }
    public int UsedPercent => TotalDriveMB > 0 
        ? (int)Math.Round(UsedDriveMB / TotalDriveMB * 100) 
        : 0;
}
