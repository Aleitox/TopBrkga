using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Brkga;
using Main.BrkgaTop;
using Main.Factory;
using Main.FileIO;

namespace Main
{
    static class Program
    {
        static void Main(string[] args)
        {
            var fileManager = new FileManager(ConfigurationManager.AppSettings["fileNameTestDistances"], ConfigurationManager.AppSettings["filePath"]);
            var caso = fileManager.ReadFile();

            var problemProvider = ProblemProviderFactory.CreateProblemProvider(caso);
            var problemDecoder = new ProblemDecoder(problemProvider);
            var populationGenerator = new PopulationGenerator(problemDecoder, problemProvider.GetAmountOfNonProfitDestinations());
            var problemManager = new ProblemManager(populationGenerator, true);
            var brkga = new Brkga.Brkga(problemManager);

            brkga.Start();
        }
    }
}
