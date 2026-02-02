-- USE [Log] -- Removed: Connection already targets the correct database
-- GO

CREATE OR ALTER PROCEDURE monitoring.spGetDbSizeStats
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
