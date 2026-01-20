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
        DB_NAME(mf.database_id) AS DatabaseName,
        mf.name AS LogicalFileName,
        ISNULL(fg.name, 'N/A') AS FileGroup,
        mf.physical_name AS PhysicalFileName,
        mf.type_desc AS FileType,
        mf.size * 8.0 / 1024 AS DatabaseFileSizeMB,
        CASE
            WHEN mf.max_size = -1 THEN 'Unlimited'
            WHEN mf.max_size = 268435456 THEN 'Unlimited'
            ELSE CAST(mf.max_size * 8.0 / 1024 AS VARCHAR(20)) + ' MB'
        END AS MaxSize,
        CASE
            WHEN mf.is_percent_growth = 1 THEN CAST(mf.growth AS VARCHAR(10)) + ' %'
            ELSE CAST(mf.growth * 8.0 / 1024 AS VARCHAR(10)) + ' MB'
        END AS AutogrowSize,
        vs.volume_mount_point AS Drive,
        vs.total_bytes / 1024 / 1024 / 1024 AS TotalDriveGB,
        vs.available_bytes / 1024 / 1024 / 1024 AS FreeDriveGB,
        CAST((vs.available_bytes * 100.0 / vs.total_bytes) AS DECIMAL(5,2)) AS FreePercent
    FROM sys.master_files mf
    LEFT JOIN sys.filegroups fg ON mf.data_space_id = fg.data_space_id AND mf.database_id = DB_ID()
    CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.file_id) vs
    ORDER BY mf.database_id, mf.file_id;
END
GO
