using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Main.Repositories;
using System.Collections.Generic;
using Main.Entities;
using Main.Factory;
using System.Linq;
using Main.BrkgaTop;
using System.Diagnostics;

namespace TesisUnitTests
{
    [TestClass]
    public class Test_Hash
    {
        [TestMethod]
        public void TestMethod1()
        {

            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var intancesIds = new List<int> { 392 };

            var intances = new List<Main.Entities.Instance>();
            foreach (var instanceId in intancesIds)
            {
                intances.Add(instanceRepository.GetById(instanceId));
            }
            var simpleSolutions = PaperConfigsFactory.GetSomeSolutions(intances, 2000, 2001, 1);
            foreach (var sol in simpleSolutions)
            {
                var hash = sol.GetHash();
            }
        }

        [TestMethod]
        public void TestMethod2()
        {

            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var instanceId = 390;
            var intance = instanceRepository.GetById(instanceId);
            var simpleEncodedSolutions = PaperConfigsFactory.GetSomeSolutionsOneDecoder(intance, 2000, 1, "Simple");

            var countdisf = 0;
            foreach (var es in simpleEncodedSolutions)
            {
                if (simpleEncodedSolutions.Any(o => o.GetSolution.Id < es.GetSolution.Id && o.StrongHash() == es.StrongHash() && es.OldHash() != o.OldHash()))
                    countdisf++;
            }


            var a1 = simpleEncodedSolutions[0].OldHash();
            var a2 = simpleEncodedSolutions[0].StrongHash();
            var b1 = simpleEncodedSolutions[1].OldHash();
            var b2 = simpleEncodedSolutions[1].StrongHash();


            var orderedEncodedSolutions = simpleEncodedSolutions.OrderBy(e => e.StrongHash());
            var total = orderedEncodedSolutions.Count();


            var groupsStrong = simpleEncodedSolutions.GroupBy(x => x.StrongHash()).Where(g => g.Count() > 1);
            var groupsOld = simpleEncodedSolutions.GroupBy(x => x.OldHash()).Where(g => g.Count() > 1);            

            var countStrong = simpleEncodedSolutions.GroupBy(x => x.StrongHash()).Select(y => y.First()).Count();
            var countOld = simpleEncodedSolutions.GroupBy(x => x.OldHash()).Select(y => y.First()).Count();

        }

        [TestMethod]
        public void TestMethod3()
        {
            // Para 390, 400 y 776. Sample 5000 soluciones. Con deco simple (quiza despues con deco goloso)
            /*
             Medir:
             1. Cuantas unicas se generan con Strong
             2. Cuantas unicas se generan con Old
             3. Cuantos clientes se visitan en promedio
             4. Cuantos clientes hay en total             
             */

            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var hashTestsRepository = new HashTestsRepository(TopEntitiesManager.GetContext());

            var instances = new List<int>() { 390, 400, 776 };
            foreach (var instanceId in instances)
            {
                var intance = instanceRepository.GetById(instanceId);
                var decoder1 = "Simple";
                var decoder2 = "Greedy";

                var decos = new List<string>() { decoder1, decoder2 };
                foreach (var decoder in decos)
                {
                    var sampleSizes = new List<int>() { 100, 1000, 5000, 20000 };
                    foreach (var sampleSize in sampleSizes)
                    {
                        var solutions = PaperConfigsFactory.GetSomeSolutionsOneDecoder(intance, 2000, sampleSize, decoder);

                        // 0. Total
                        var total = solutions.Count();

                        //var orderedSolutions = solutions.OrderBy(e => e.StrongHash()).ToList();
                        //var uniques = new List<EncodedSolution>();
                        //var amount = 1;
                        //uniques.Add(orderedSolutions[0]);
                        //for (var i = 1; i< orderedSolutions.Count; i++)
                        //{
                        //    if (orderedSolutions[i].StrongHash() != orderedSolutions[i - 1].StrongHash())
                        //    {
                        //        uniques.Add(orderedSolutions[i]);
                        //        amount++;
                        //    }
                        //}
                        //var uniqueCount = uniques.Count();
                        //var orderdUnique = uniques.OrderByDescending(x => x.GetSolution.GetCurrentProfit);

                        var uniquesStrong = solutions.GroupBy(x => x.StrongHash()).Select(y => y.First());
                        var best = uniquesStrong.OrderByDescending(x => x.GetSolution.GetCurrentProfit);
                        var bestprofit = best.First().GetSolution.GetCurrentProfit;

                        var desconfio = uniquesStrong.Count();

                        var uniquesOld = solutions.GroupBy(x => x.OldHash()).Select(y => y.First());
                        //1. Strong
                        var countStrong = uniquesStrong.Count();
                        //2. Old
                        var countOld = uniquesOld.Count();

                        decimal promedioClientesVisitados = 0;

                        foreach (var e in uniquesStrong)
                        {
                            var visitados = e.GetSolution.VehicleFleet.Vehicles.Sum(v => v.Route.RouteLenght());
                            promedioClientesVisitados += visitados;
                        }
                        //3. prom visitados
                        promedioClientesVisitados = promedioClientesVisitados / countStrong;
                        //4. total
                        var clientes = intance.Destinies.Count - 2;

                        var hashTest = new HashTest();

                        hashTest.InstanceId = instanceId;
                        hashTest.Decodificador = decoder;
                        hashTest.ClientesTotal = clientes;
                        hashTest.UnicidadStrongHash = countStrong;
                        hashTest.UnicidadWeakHash = countOld;
                        hashTest.SampleSize = sampleSize;
                        hashTest.ClientesVisitadosAvg = promedioClientesVisitados;
                        hashTest.BestProfit = Convert.ToInt32(bestprofit);
                        hashTestsRepository.SaveHashTest(hashTest);
                    }                    
                }
            }            
        }
    }
}
