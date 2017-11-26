/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [SolutionId]
      ,[Algorithim Code]
	  ,Run
      ,[InstanceId]
      ,[Author]
      ,[Name]
      ,[Set]
      ,[DestiniesCount]
      ,[Vehicles]
      ,[TMax]
      ,[TimeElapsedInMilliseconds]
      ,[Total Profit]
      ,[AHS_Slow_Vns_Feasible_Max]
      ,[TMH]
      ,[CGH]
      ,[CreationDate]
      ,[Fase]
  FROM [TOP].[dbo].[vSolutions]
  where --fase = 3
  [InstanceId] = 400


  Select [InstanceId], [Algorithim Code], count(1) as SampleSize, Max([Total Profit]) as [Max Profit], Min([Total Profit]) as [Min Profit], Avg([Total Profit]) as [Avg Profit], Max([AHS_Slow_Vns_Feasible_Max]) as [AHS], Max([TMH]) as [TMH], Max([CGH]) as[CGH] from [dbo].[vSolutions]
  where fase = 3 
  and [InstanceId] = 400
  group by [Algorithim Code], fase , [InstanceId]
  order by [Algorithim Code], [InstanceId]


  --sp_Delete_Solution 2024

 -- exec [sp_Delete_Solution_In_Range] 2025, 2029;
 
 -- exec [sp_Delete_Solution_In_Fase] 1;