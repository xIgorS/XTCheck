USE [master];
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LOG_FI_ALMT')
BEGIN
    CREATE DATABASE [LOG_FI_ALMT];
END
GO

USE [LOG_FI_ALMT];
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'administration')
BEGIN
    EXEC('CREATE SCHEMA [administration]');
END
GO

-- Create Type
IF NOT EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'ReplayAdjAtCoreSet')
BEGIN
    CREATE TYPE [administration].[ReplayAdjAtCoreSet] AS TABLE(
        [FlowIdDerivedFrom] [bigint] NOT NULL,
        [FlowId] [bigint] NOT NULL,
        [PnlDate] [date] NOT NULL,
        [WithBackdated] [bit] NOT NULL DEFAULT ((1)),
        [SkipCoreProcess] [bit] NOT NULL DEFAULT ((0)),
        [Droptabletpm] [bit] NOT NULL DEFAULT ((1))
    );
END
GO

-- Create Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[administration].[Flows]') AND type in (N'U'))
BEGIN
    CREATE TABLE [administration].[Flows](
        [FlowId] [bigint],
        [FlowIdDerivedFrom] [bigint] NULL,
        [BusinessDataTypeId] [smallint] NULL,
        [FeedSourceId] [smallint] NULL,
        [PnlDate] [date] NOT NULL,
        [ReportingDate] [date] NULL,
        [FileName] [varchar](500) NULL,
        [ArrivalDate] [datetime] NULL,
        [PackageGuid] [uniqueidentifier] NULL,
        [CurrentStep] [varchar](50) NULL,
        [IsFailed] [varchar](50) NULL,
        [TypeOfCalculation] [varchar](50) NULL
    );
END
GO

-- Populate Table
TRUNCATE TABLE [administration].[Flows];

INSERT INTO [administration].[Flows] (
    [FlowId], [FlowIdDerivedFrom], [BusinessDataTypeId], [FeedSourceId], 
    [PnlDate], [ReportingDate], [FileName], [ArrivalDate], 
    [PackageGuid], [CurrentStep], [IsFailed], [TypeOfCalculation]
) VALUES 
(302404279, 302846881, 7, 8, '2026-01-29', NULL, NULL, '2026-01-29 23:37:11.597', '65C13E9B-8FF3-4DAC-A2B2-B1373C70422A', 'Completed', '0', 's'),
(302392144, 302346881, 7, 11, '2026-01-29', NULL, NULL, '2026-01-29 23:37:11.597', '17DAB1B3-B164-4882-A1DE-91FA45423203', 'Completed', '0', 's'),
(302390774, 302346881, 7, 9, '2026-01-29', NULL, NULL, '2026-01-29 23:37:11.597', '4B19BF16-137D-4D34-80A1-5D70D0D2A026', 'Completed', '0', 's'),
(302380547, 302346881, 7, 9, '2026-01-29', NULL, NULL, '2026-01-29 23:37:11.597', '55CF0839-D48F-4FE2-88EF-28AEEFBA22A9', 'Completed', '0', 's'),
(302396627, 302346881, 7, 9, '2026-01-29', NULL, NULL, '2026-01-29 23:37:11.597', 'FDAF0FB3-720F-459D-BBB3-1D9CE4083044', 'Completed', '0', 's'),
(302401509, 302346881, 7, 8, '2026-01-29', NULL, NULL, '2026-01-29 23:37:11.597', '486948F3-26AC-4C87-8734-094399C30D89', 'Completed', '0', 's'),
(302022167, 301892520, 1, 98, '2026-01-27', NULL, NULL, '2026-01-27 23:47:08.007', '44AA25CA-EF09-4597-B5DD-FB478A2E7DB1', 'Completed', '0', 'H'),
(301586174, 301580930, 1, 21, '2026-01-22', NULL, '/dfs/root/TPS/AppMkt...', '2026-01-23 00:00:00.000', 'B25CE362-D927-41E9-817C-60BC518786A0', 'Completed', '0', 'P'),
(302394980, 302346881, 7, 8, '2026-01-29', NULL, NULL, '2026-01-29 23:37:11.597', 'BEBF7C34-8E6F-4D1E-9593-BFFF95180ADA', 'Completed', '0', 's'),
(302374045, 302346881, 7, 148, '2026-01-29', NULL, NULL, '2026-01-29 23:37:11.597', 'E052B124-5CB9-446F-A4C1-60BB6149798D', 'Completed', '0', 's');
GO
