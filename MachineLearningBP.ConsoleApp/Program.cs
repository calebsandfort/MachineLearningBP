using Abp;
using Abp.Dependency;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Services.Sports.Nba;
using MachineLearningBP.Shared.CommandRunner;
using MachineLearningBP.Shared.Dtos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MachineLearningBP.ConsoleApp
{
    class Program
    {
        private static Timer pingTimer;

        static void Main(string[] args)
        {
            using (var bootstrapper = AbpBootstrapper.Create<MachineLearningBPConsoleAppModule>())
            {
                bootstrapper.Initialize();

                using (var sheetUtility = new SheetUtility())
                {

                    Console.WriteLine();
                    Console.WriteLine(" 1 - Authorize");
                    Console.WriteLine(" 2 - Ping");
                    Console.WriteLine(" 3 - NbaPoints.FindOptimalParameters");
                    Console.WriteLine(" 4 - NbaPoints.GeneticOptimize");
                    Console.WriteLine(" 5 - NbaPoints.AnnealingOptimize");
                    Console.WriteLine(" 6 - NbaAtsTree.BuildDecisionTree");
                    Console.WriteLine(" 7 - CommandRunner.PythonTest");

                    bool keepGoing = true;
                    Stopwatch timer = new Stopwatch();

                    while (keepGoing)
                    {
                        Console.Write(" Enter option: ");
                        char objectOptionChar = Console.ReadKey(false).KeyChar;
                        Console.WriteLine();

                        if (objectOptionChar == 'q')
                        {
                            keepGoing = false;
                        }
                        else
                        {
                            int objectOptionInt = Int32.Parse(objectOptionChar.ToString());

                            switch (objectOptionInt)
                            {
                                case 1:
                                    sheetUtility.Authorize();
                                    break;
                                case 2:
                                    SendPing();

                                    pingTimer = new System.Timers.Timer();
                                    pingTimer.Interval = 30000 * 1;

                                    // Hook up the Elapsed event for the timer. 
                                    pingTimer.Elapsed += PingTimer_Elapsed; ;

                                    // Have the timer fire repeated events (true is the default)
                                    pingTimer.AutoReset = true;

                                    // Start the timer
                                    pingTimer.Enabled = true;
                                    break;
                                case 3:
                                    using (var _nbaPointsExampleDomainService = bootstrapper.IocManager.ResolveAsDisposable<INbaPointsExampleDomainService>())
                                    {
                                        _nbaPointsExampleDomainService.Object.FindOptimalParameters(true);
                                    }
                                    break;
                                case 4:
                                    using (var _nbaPointsExampleDomainService = bootstrapper.IocManager.ResolveAsDisposable<INbaPointsExampleDomainService>())
                                    {
                                        GeneticOptimizeInput input = new GeneticOptimizeInput();
                                        input.GuessMethod = KNearestNeighborsGuessMethods.WeightedKnn;
                                        input.WeightMethod = KNearestNeighborsWeightMethods.InverseWeight;
                                        input.Trials = 25;
                                        input.K = 40;
                                        input.popsize = 50;
                                        input.step = 1;
                                        input.mutprob = .20;
                                        input.elite = .20;
                                        input.maxiter = 100;

                                        _nbaPointsExampleDomainService.Object.GeneticOptimize(input);
                                    }
                                    break;
                                case 5:
                                    using (var _nbaPointsExampleDomainService = bootstrapper.IocManager.ResolveAsDisposable<INbaPointsExampleDomainService>())
                                    {
                                        AnnealingOptimizeInput input = new AnnealingOptimizeInput();
                                        input.GuessMethod = KNearestNeighborsGuessMethods.WeightedKnn;
                                        input.WeightMethod = KNearestNeighborsWeightMethods.InverseWeight;
                                        input.Trials = 25;
                                        input.K = 40;
                                        input.T = 10000;
                                        input.step = 1;
                                        input.cool = .95;

                                        _nbaPointsExampleDomainService.Object.AnnealingOptimize(input);
                                    }
                                    break;
                                case 6:
                                    using (var _nbaAtsTreeExampleDomainService = bootstrapper.IocManager.ResolveAsDisposable<INbaAtsTreeExampleDomainService>())
                                    {
                                        _nbaAtsTreeExampleDomainService.Object.BuildDecisionTree();
                                    }
                                    break;
                                case 7:
                                    using (StreamWriter testPyFile = new StreamWriter("C:\\Users\\csandfort\\Documents\\Visual Studio 2017\\Projects\\MachineLearningBP\\MachineLearningBP.CollectiveIntelligence\\DomainServices\\Algorithms\\Scripts\\Python\\test.py", false))
                                    {
                                        testPyFile.WriteLine("print(\"I'm a python file\")");
                                        testPyFile.Close();
                                    }

                                    using (var _commandRunner = bootstrapper.IocManager.ResolveAsDisposable<ICommandRunner>())
                                    {
                                        _commandRunner.Object.RunCmd("python", "C:\\Users\\csandfort\\Documents\\Visual Studio 2017\\Projects\\MachineLearningBP\\MachineLearningBP.CollectiveIntelligence\\DomainServices\\Algorithms\\Scripts\\Python\\GowerTest.py");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private static void PingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendPing();
        }

        private static void SendPing()
        {
            Console.WriteLine(" Pinging...");

            var client = new RestClient("http://localhost/MachineLearningBP.Web/");
            var request = new RestRequest("ConsoleSignalR/WriteLine", Method.POST);

            request.RequestFormat = DataFormat.Json;

            ConsoleWriteLineInput consoleWriteLineInput = ConsoleWriteLineInput.Create("Ping");

            request.AddJsonBody(new { input = consoleWriteLineInput });

            RestResponse response = (RestResponse)client.Execute(request);
        }
    }
}
