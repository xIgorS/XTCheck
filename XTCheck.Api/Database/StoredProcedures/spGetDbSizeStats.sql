USE [Log]
GO

IF OBJECT_ID('monitoring.spGetDbSizeStats', 'P') IS NOT NULL
    DROP PROCEDURE monitoring.spGetDbSizeStats
GO

CREATE PROCEDURE monitoring.spGetDbSizeStats
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [ExtTime],
        [InstanceName],
        [DatabaseName],
        [LogicalFileName],
        [FileGroup],
        [PhysicalFileName],
        [FileType],
        [AllocatedSpaceMB],
        [UsedSpaceMB],
        [FreeSpaceMB],
        [UsedPercent],
        [MaxSizeMB],
        [AutogrowSize],
        [TotalDriveMB],
        [FreeDriveMB],
        [FreeDrivePercent]
    FROM [monitoring].[DBSizePlusDisk]
END
GO
