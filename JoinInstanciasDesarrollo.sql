/****** Script for SelectTopNRows command from SSMS  ******/
SELECT b.Best, es.AHS_Slow_Vns_Feasible_Max, es.CGH, es.TMH
  FROM [TOP].[dbo].[ExternalSolutionsBDM] b
  join [dbo].[Instances] i on i.Name = b.InstanceName
  join [dbo].[ExternalSolutions] es on es.InstanceName = i.Name
  where i.Id in (628, 407, 400, 776, 721, 532)
  --where [Best] > [MAzBest] AND [Best] > [VNSslowZbest] AND [Best] >[ACOseqZbest]