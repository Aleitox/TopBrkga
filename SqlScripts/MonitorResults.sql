-- Res finales:
  Select 
	--i.Id,
	i.Name, 
	i.DestiniesCount as [Vertices], 
	i.Vehicles as [Vehiculos], 
	i.TMax, 
	--a.[Sample Size], 
	--a.[Algorithim Code], 
	a.[Avg Time ms],
	[Min Profit], 
	[Avg Profit],
	a.[Max Profit], 
	a.AHS,
	a.CGW,
	a.TMH  
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
		vs.AHS_Slow_Vns_Feasible_Max as [AHS],
		vs.CGH as [CGW],
		vs.TMH
		from [dbo].[vSolutions] vs
		--join dbo.Instances i on i.id = vs.InstanceId
	  where fase = 9 and [InstanceId] = 776 --and [Algorithim Code] = 'Decoder Greedy Alone'
	  group by [Algorithim Code], fase , [InstanceId], vs.AHS_Slow_Vns_Feasible_Max, vs.CGH, vs.TMH
	  --order by [Algorithim Code], [InstanceId]
  ) a
  join dbo.Instances i on i.id = a.InstanceId 
  --where name = 'p4.2.q'
  order by name asc--i.DestiniesCount asc, i.Vehicles asc, a.[Algorithim Code]

-- Resultado parciales
  Select 
	--i.Id,
	i.Name, 
	i.DestiniesCount as [Vertices], 
	i.Vehicles as [Vehiculos], 
	i.TMax, 
	a.[Sample Size], 
	a.[Algorithim Code], 
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
	  where fase = 9 and [InstanceId] = 776  --and [Algorithim Code] = 'Decoder Greedy Alone'
	  group by [Algorithim Code], fase, [InstanceId]
	  --order by [Algorithim Code], [InstanceId]
  ) a
  join dbo.Instances i on i.id = a.InstanceId 
  order by i.DestiniesCount asc, i.Vehicles asc, a.[Algorithim Code]


 -- sp_Delete_Solution 2024

 -- exec [sp_Delete_Solution_In_Range] 6314, 6323;
 
 -- exec [sp_Delete_Solution_In_Fase] 8;

 /*
 select * from vSolutions
 where 522 = instanceid and fase = 3
 order by solutionid asc
 */
 


 Select 
		vs.SolutionId,
		InstanceId, 
		[Algorithim Code], 
		[Run],
		TimeElapsedInMilliseconds,
		[Total Profit], 
		BestProfit,
		vs.ProfitEvolution
		from [dbo].[vSolutions] vs
		join dbo.Instances i on i.id = vs.InstanceId
	  where fase = 6 and [Algorithim Code] = 'MI.250;MNC.50;PS.250;EP.0,15;MP.0,05;EGC.50;HEU.;TOP.0;MI.G' and i.Name = 'p7.4.t'
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