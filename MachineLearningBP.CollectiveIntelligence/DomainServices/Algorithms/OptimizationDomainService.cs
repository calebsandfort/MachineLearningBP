using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.Shared.Dtos;
using Abp.BackgroundJobs;
using MachineLearningBP.Shared.GuerillaTimer;
using MachineLearningBP.Shared.CommandRunner;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public class OptimizationDomainService : BaseDomainService, IOptimizationDomainService
    {
        Random random { get; set; }
        public readonly ICommandRunner _commandRunner;

        #region Constructor
        public OptimizationDomainService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager, ICommandRunner commandRunner)
            : base(sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
            random = new Random(DateTime.Now.Millisecond);
            _commandRunner = commandRunner;
        }
        #endregion

        #region AnnealingOptimize
        public OptimizeResult AnnealingOptimize(IAnnealingOptimizeInput input)
        {
            return AnnealingOptimize(input.domain, input.costf, input.T, input.cool, input.step);
        }

        public OptimizeResult AnnealingOptimize(List<OptimizationRange> domain, Func<List<int>, double> costf, double T = 10000, double cool = 0.95, int step = 1)
        {
            //Initialize the values randomly
            List<int> vec = new List<int>();
            for (int i = 0; i < domain.Count; i++)
            {
                OptimizationRange range = domain[i];
                vec.Add(random.Next(range.Lower, range.Upper + 1));
            }

            while(T > .1)
            {
                using (GuerillaTimer timer = new GuerillaTimer(this._consoleHubProxy, $"T: {T}"))
                {
                    //Choose one of the indices
                    int i = random.Next(0, domain.Count);

                    //Choose a direction to change it
                    int dir = random.Next(-step, step + 1);

                    //Create a new list with one of the values changed
                    List<int> vecb = vec.ToList();
                    vecb[i] += dir;
                    if (vecb[i] < domain[i].Lower) vecb[i] = domain[i].Lower;
                    else if (vecb[i] > domain[i].Upper) vecb[i] = domain[i].Upper;

                    //Calculate the current cost and the new cost
                    Double ea = costf(vec);
                    Double score = ea;
                    Double eb = costf(vecb);
                    Double p = Math.Pow(Math.E, (-eb - ea) / T);

                    //Is it better, or does it make the probability cutoff?
                    if (eb < ea || random.NextDouble() < p)
                    {
                        vec = vecb.ToList();
                        score = eb;
                    }

                    T *= cool;

                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Current best score: {score}, {String.Join(",", vec)}"));
                }
            }

            return new OptimizeResult { Vec = vec };
        }
        #endregion

        #region GeneticOptimize
        public OptimizeResult GeneticOptimize(IGeneticOptimizeInput input)
        {
            return GeneticOptimize(input.domain, input.costf, input.popsize, input.step, input.mutprob, input.elite, input.maxiter);
        }

        public OptimizeResult GeneticOptimize(List<OptimizationRange> domain, Func<List<int>, double> costf, int popsize = 50, int step = 1, double mutprob = 0.2, double elite = 0.2, int maxiter = 100)
        {
            //Mutation Operation
            Func<List<int>, List<int>> mutate = (v) =>
            {
                List<int> mutated = new List<int>();
                int i = random.Next(0, domain.Count);

                if(random.NextDouble() < .5 && v[i] > domain[i].Lower)
                {
                    mutated = v.Select((x, idx) => idx == i ? (x - step) : x).ToList();
                }
                else if(v[i] < domain[i].Upper)
                {
                    mutated = v.Select((x, idx) => idx == i ? (x + step) : x).ToList();
                }

                return mutated;
            };

            Func<List<int>, List<int>, List<int>> crossover = (r1, r2) =>
            {
                List<int> crossed = new List<int>();
                int i = random.Next(0, domain.Count - 1);

                crossed.AddRange(r1.Take(i));
                crossed.AddRange(r2.Skip(i));

                return crossed;
            };

            List<List<int>> pop = new List<List<int>>();

            //Build the initial population
            for (int j = 0; j < popsize; j++)
            {
                List<int> vec = new List<int>();
                for (int i = 0; i < domain.Count; i++)
                {
                    OptimizationRange range = domain[i];
                    vec.Add(random.Next(range.Lower, range.Upper + 1));
                }
                pop.Add(vec);
            }

            int topelite = (int)(elite * popsize);

            //Main loop
            List<OptimizationScore> scores = new List<OptimizationScore>();
            for(int i = 0; i < maxiter; i++)
            {
                using (GuerillaTimer timer = new GuerillaTimer(this._consoleHubProxy, $"Iteration: {i}"))
                {

                    scores = new List<OptimizationScore>();
                    foreach (List<int> v in pop)
                    {
                        scores.Add(new OptimizationScore { Cost = costf(v), Vec = v });
                    }
                    scores = scores.OrderBy(x => x.Cost).ToList();

                    //No need on last iteration
                    if (i < (maxiter - 1))
                    {
                        List<List<int>> ranked = scores.Select(x => x.Vec).ToList();

                        //Start with the pure winners
                        pop = ranked.Take(topelite).ToList();

                        //Add mutated and bred forms of the winners
                        while (pop.Count < popsize)
                        {
                            //Mutation
                            if (random.NextDouble() < mutprob)
                            {
                                int c = random.Next(0, topelite);
                                pop.Add(mutate(ranked[c]));
                            }
                            //Crossover
                            else
                            {
                                int c1 = random.Next(0, topelite);
                                int c2 = random.Next(0, topelite);
                                pop.Add(crossover(ranked[c1], ranked[c2]));
                            }
                        }
                    }

                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Current best score: {scores.First().Cost}, {String.Join(",", scores.First().Vec)}"));
                }
            }

            return new OptimizeResult { Vec = scores.First().Vec };
        }
        #endregion
    }
}
