/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [SolutionId]
      ,[Id]
      ,[Vehicle Number]
      ,[TMax]
      ,[Distance]
      ,[SumProfit]
      ,[Order]
      ,[X]
      ,[Y]
      ,[Profit]
  FROM [TOP].[dbo].[vVehiclesRoute]
  where [SolutionId] = 12932


  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [SolutionId]
      ,[Algorithim Code]
      ,[Run]
      ,[InstanceId]
      ,[Author]
      ,[Name]
      ,[Set]
      ,[DestiniesCount]
      ,[Vehicles]
      ,[TMax]
      ,[TimeElapsedInMilliseconds]
      ,[Total Profit]
      ,[VNS]
      ,[TMH]
      ,[CGH]
      ,[ACOseqZbest]
      ,[MAzBest]
      ,[Best]
      ,[CreationDate]
      ,[Fase]
      ,[ProfitEvolution]
  FROM [TOP].[dbo].[vSolutions]
  where [InstanceId] = 400


  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[DestiniesCount]
      ,[DestinationId]
      ,[Vehicles]
      ,[TMax]
      ,[X]
      ,[Y]
      ,[Profit]
      ,[Description]
  FROM [TOP].[dbo].[vInstances]
  where id = 400

  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Author]
      ,[Set]
      ,[Name]
      ,[DestiniesCount]
      ,[Vehicles]
      ,[TMax]
  FROM [TOP].[dbo].[Instances]
  where id = 400