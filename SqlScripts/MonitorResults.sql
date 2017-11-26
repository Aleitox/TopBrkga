/****** Script for SelectTopNRows command from SSMS  ******/
/*
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
  */


  Select 
	i.Name, 
	i.DestiniesCount as [Vertices], 
	i.Vehicles as [Vehiculos], 
	i.TMax, 
	--a.[Sample Size], 
	--a.[Algorithim Code], 
	a.[Avg Time ms],
	a.[Max Profit], 
	[Min Profit], 
	[Avg Profit],
	convert(decimal(18,2),convert(decimal(18,2), a.[Avg Profit]) / convert(decimal(18,2), a.[Previous works Max Profit])) as [Indice de Efectividad]
  , [Previous works Max Profit]
  from 
    (
	  Select 
		InstanceId, 
		[Algorithim Code], 
		count(1) as [Sample Size], 
		avg(vs.TimeElapsedInMilliseconds) as [Avg Time ms],
		Max([Total Profit]) as [Max Profit], 
		Min([Total Profit]) as [Min Profit], 
		Avg([Total Profit]) as [Avg Profit], 
		Max(BestProfit) as [Previous works Max Profit]
		from [dbo].[vSolutions] vs
		--join dbo.Instances i on i.id = vs.InstanceId
	  where fase = 5 --and [Algorithim Code] = 'Decoder Greedy Alone'
	  group by [Algorithim Code], fase , [InstanceId]
	  --order by [Algorithim Code], [InstanceId]
  ) a
  join dbo.Instances i on i.id = a.InstanceId 
  order by i.DestiniesCount asc, i.Vehicles asc, a.[Algorithim Code]


 -- sp_Delete_Solution 2024

 -- exec [sp_Delete_Solution_In_Range] 4509, 4708;
 
 -- exec [sp_Delete_Solution_In_Fase] 4;

 /*
 select * from vSolutions
 where 522 = instanceid and fase = 3
 order by solutionid asc
 */




 Select 
		InstanceId, 
		[Algorithim Code], 
		[Run],
		TimeElapsedInMilliseconds,
		[Total Profit], 
		BestProfit,
		vs.ProfitEvolution
		from [dbo].[vSolutions] vs
		join dbo.Instances i on i.id = vs.InstanceId
	  where fase in (5, 6, 7) --and [Algorithim Code] = 'Decoder Greedy Alone'
	  order by fase, InstanceId, [Algorithim Code]










 /*
-- Instanceas para indice de efectividad
SELECT TOP (1000) 
	[Author] as [Autor]
    ,[Name] as [Instancea]
    ,[DestiniesCount] as [Vertices]
    ,[Vehicles] as [Vehículos]
    ,[TMax]
FROM [TOP].[dbo].[Instances]
where id in (628, 407, 400, 776, 721, 532)


 */