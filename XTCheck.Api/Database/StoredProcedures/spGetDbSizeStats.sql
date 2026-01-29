USE [Log]
GO

IF OBJECT_ID('dbo.spGetDbSizeStats', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spGetDbSizeStats
GO

CREATE PROCEDURE dbo.spGetDbSizeStats
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
