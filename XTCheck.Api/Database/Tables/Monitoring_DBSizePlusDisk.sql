IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'monitoring')
BEGIN
    EXEC('CREATE SCHEMA [monitoring]')
END
GO

IF OBJECT_ID('[monitoring].[DBSizePlusDisk]', 'U') IS NOT NULL
DROP TABLE [monitoring].[DBSizePlusDisk]
GO

CREATE TABLE [monitoring].[DBSizePlusDisk](
    [ExtTime] [datetime2](0) NULL,
    [InstanceName] [nvarchar](128) NULL,
    [DatabaseName] [nvarchar](128) NULL,
    [LogicalFileName] [nvarchar](128) NULL,
    [FileGroup] [nvarchar](128) NULL,
    [PhysicalFileName] [nvarchar](512) NULL,
    [FileType] [nvarchar](10) NULL,
    [AllocatedSpaceMB] [decimal](18, 6) NULL,
    [UsedSpaceMB] [decimal](18, 0) NULL,
    [FreeSpaceMB] [decimal](18, 0) NULL,
    [UsedPercent] [int] NULL,
    [MaxSizeMB] [decimal](18, 0) NULL,
    [AutogrowSize] [nvarchar](50) NULL,
    [TotalDriveMB] [decimal](18, 0) NULL,
    [FreeDriveMB] [decimal](18, 0) NULL,
    [FreeDrivePercent] [decimal](5, 2) NULL
) ON [PRIMARY]
GO

INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_PRIMARY1', 'PRIMARY', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 98304.000000, 4804, 93499, 4, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_log', 'N/A', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'LOG', 870400.000000, 746, 869653, 0, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_PRIMARY2', 'PRIMARY', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 98304.000000, 4772, 93532, 4, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_11', 'DATAFACT_1', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 1024000.000000, 967621, 56378, 94, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_12', 'DATAFACT_2', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 1024000.000000, 970863, 53136, 94, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_21', 'DATAFACT_2', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 880640.000000, 843742, 36897, 95, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_22', 'DATAFACT_2', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 880640.000000, 844014, 36625, 95, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_31', 'DATAFACT_3', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 1024000.000000, 885717, 138282, 86, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_32', 'DATAFACT_3', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 1024000.000000, 890365, 133634, 86, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_41', 'DATAFACT_4', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 970000.000000, 895472, 74527, 92, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
INSERT INTO [monitoring].[DBSizePlusDisk] VALUES ('2026-01-27 16:09:00', 'EURVOID07304\INSTSQL2017', 'DTM_FI', 'DTM_FI_DATAFACT_42', 'DATAFACT_4', 'C:\Program Files\Delphix\DelphixConnector\4229d6...', 'ROWS', 880640.000000, 835744, 44895, 94, 66060288, '512.000000 MB', 66060288, 39903422, 60.40);
