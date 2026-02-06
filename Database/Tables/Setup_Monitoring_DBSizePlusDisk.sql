USE [LOG_FI_ALMT];
GO

-- Create Schema
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'monitoring')
BEGIN
    EXEC('CREATE SCHEMA [monitoring]');
END
GO

-- Create Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[monitoring].[DBSizePlusDisk]') AND type in (N'U'))
BEGIN
    CREATE TABLE [monitoring].[DBSizePlusDisk](
        [ExtTime] [datetime] NOT NULL,
        [InstanceName] [varchar](100) NULL,
        [DatabaseName] [varchar](100) NULL,
        [LogicalFileName] [varchar](100) NULL,
        [FileGroup] [varchar](100) NULL,
        [PhysicalFileName] [varchar](500) NULL,
        [FileType] [varchar](50) NULL,
        [AllocatedSpaceMB] [float] NULL,
        [UsedSpaceMB] [float] NULL,
        [FreeSpaceMB] [float] NULL,
        [UsedPercent] [float] NULL,
        [MaxSizeMB] [float] NULL,
        [AutogrowSize] [varchar](50) NULL,
        [TotalDriveMB] [float] NULL,
        [FreeDriveMB] [float] NULL,
        [FreeDrivePercent] [float] NULL
    );
END
GO

-- Populate Table
-- Truncate to avoid duplicates on re-run
TRUNCATE TABLE [monitoring].[DBSizePlusDisk];

INSERT INTO [monitoring].[DBSizePlusDisk] (
    [ExtTime], [InstanceName], [DatabaseName], [LogicalFileName], [FileGroup], 
    [PhysicalFileName], [FileType], [AllocatedSpaceMB], [UsedSpaceMB], [FreeSpaceMB], 
    [UsedPercent], [MaxSizeMB], [AutogrowSize], [TotalDriveMB], [FreeDriveMB], [FreeDrivePercent]
) VALUES 
('2026-02-05 16:07:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_11', 'DATAFACT_1', 'C:\Program Files\Delphix\DelphixConnector\4229d6\DTM_FI_DATAFACT_11', 'ROWS', 1024000, 863305, 160693, 84, 66060288, '512.000000 MB', 66060288, 39903422, 60.40),
('2026-02-05 16:07:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_12', 'DATAFACT_2', 'C:\Program Files\Delphix\DelphixConnector\4229d6\DTM_FI_DATAFACT_12', 'ROWS', 1024000, 865823, 158176, 84, 66060288, '512.000000 MB', 66060288, 39903422, 60.40),
('2026-02-05 16:07:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_21', 'DATAFACT_2', 'C:\Program Files\Delphix\DelphixConnector\4229d6\DTM_FI_DATAFACT_21', 'ROWS', 880640, 744186, 136453, 84, 66060288, '512.000000 MB', 66060288, 39903422, 60.40),
('2026-02-05 16:07:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_22', 'DATAFACT_2', 'C:\Program Files\Delphix\DelphixConnector\4229d6\DTM_FI_DATAFACT_22', 'ROWS', 880640, 751979, 128660, 85, 66060288, '512.000000 MB', 66060288, 39903422, 60.40),
('2026-02-05 16:07:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_31', 'DATAFACT_3', 'C:\Program Files\Delphix\DelphixConnector\4229d6\DTM_FI_DATAFACT_31', 'ROWS', 1024000, 855155, 168844, 83, 66060288, '512.000000 MB', 66060288, 39903422, 60.40),
('2026-02-05 16:07:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_32', 'DATAFACT_3', 'C:\Program Files\Delphix\DelphixConnector\4229d6\DTM_FI_DATAFACT_32', 'ROWS', 1024000, 876064, 147935, 85, 66060288, '512.000000 MB', 66060288, 39903422, 60.40),
('2026-02-05 16:07:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_41', 'DATAFACT_4', 'C:\Program Files\Delphix\DelphixConnector\4229d6\DTM_FI_DATAFACT_41', 'ROWS', 970000, 751562, 218437, 77, 66060288, '512.000000 MB', 66060288, 39903422, 60.40),
('2026-02-05 16:07:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_42', 'DATAFACT_4', 'C:\Program Files\Delphix\DelphixConnector\4229d6\DTM_FI_DATAFACT_42', 'ROWS', 880640, 724193, 156446, 82, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
GO
