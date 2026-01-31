CREATE or ALTER PROCedure monitoring.GetDBSizePlusDisk
AS

DECLARE @MaxExtTime DATETIME = (select max(ExtTime) from [Log].[monitoring].[DBSizePlusDisk])


SELECT 
      [DatabaseName]
      ,[FileGroup] 
      ,CAST(AllocatedSpaceMB AS INT) AS AllocatedDBSpaceMB
      ,[UsedSpaceMB] as UsedDBSpaceMB
      ,[FreeSpaceMB] as FreeDBSpaceMB
      ,[MaxSizeMB]
      ,CASE WHEN [AutogrowSize] is null THEN 0 else 1 END AS [AutogrowEnabled]
     -- ,CASE WHEN [AutogrowSize] is null THEN FreeSpaceMB else FreeSpaceMB + FreeDriveMB END AS FreeSpace
      ,[FreeDriveMB] 
      INTO #DBSize   
  FROM [Log].[monitoring].[DBSizePlusDisk]
    WHERE [FileGroup] LIKE 'DATAFACT%'
      AND ExtTime = @MaxExtTime

--select * from #DBSize
DECLARE @PartSizeMB INT = 20248881

SELECT DatabaseName, [FileGroup], AllocatedDBSpaceMB, UsedDBSpaceMB, FreeDBSpaceMB, AutogrowEnabled, FreeDriveMB, @PartSizeMB AS PartSizeMB
    , TotalFreeSpaceMB
, CASE 
    WHEN TotalFreeSpaceMB < @PartSizeMB THEN 'CRITICAL' 
    WHEN TotalFreeSpaceMB >= @PartSizeMB AND TotalFreeSpaceMB < PartSizeMBx2 THEN 'WARNING' 
    ELSE 'OK' 
  END AS AlertLevel
FROM (

SELECT DatabaseName, [FileGroup], sum(AllocatedDBSpaceMB) as AllocatedDBSpaceMB, sum(UsedDBSpaceMB) as UsedDBSpaceMB, sum(FreeDBSpaceMB) as FreeDBSpaceMB
,CASE when sum(AutogrowEnabled) > 0 then 1 else 0 end as AutogrowEnabled, max(FreeDriveMB) as FreeDriveMB
,CASE when sum(AutogrowEnabled) > 0 then sum(FreeDBSpaceMB) + max(FreeDriveMB) else sum(FreeDBSpaceMB) end as TotalFreeSpaceMB
,@PartSizeMB * 2 as PartSizeMBx2
  FROM #DBSize
    GROUP BY DatabaseName, [FileGroup]  
) AS T 
     