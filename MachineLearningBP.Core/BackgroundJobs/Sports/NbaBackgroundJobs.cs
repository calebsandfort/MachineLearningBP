using Abp.BackgroundJobs;
using Abp.Dependency;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Services.Sports.Nba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.BackgroundJobs.Sports
{
    public class NbaPointsFindOptimalParametersBackgroundJob : BackgroundJob<bool>, ITransientDependency
    {
        private readonly INbaPointsExampleDomainService _nbaPointsExampleDomainService;

        public NbaPointsFindOptimalParametersBackgroundJob(INbaPointsExampleDomainService nbaPointsExampleDomainService)
        {
            _nbaPointsExampleDomainService = nbaPointsExampleDomainService;
        }

        public override void Execute(bool args)
        {
            this._nbaPointsExampleDomainService.FindOptimalParameters(args).GetAwaiter().GetResult();
        }
    }

    public class NbaPointsAnnealingOptimizeBackgroundJob : BackgroundJob<AnnealingOptimizeInput>, ITransientDependency
    {
        private readonly INbaPointsExampleDomainService _nbaPointsExampleDomainService;

        public NbaPointsAnnealingOptimizeBackgroundJob(INbaPointsExampleDomainService nbaPointsExampleDomainService)
        {
            _nbaPointsExampleDomainService = nbaPointsExampleDomainService;
        }

        public override void Execute(AnnealingOptimizeInput args)
        {
            this._nbaPointsExampleDomainService.AnnealingOptimize(args).GetAwaiter().GetResult();
        }
    }

    public class NbaPointsGeneticOptimizeBackgroundJob : BackgroundJob<GeneticOptimizeInput>, ITransientDependency
    {
        private readonly INbaPointsExampleDomainService _nbaPointsExampleDomainService;

        public NbaPointsGeneticOptimizeBackgroundJob(INbaPointsExampleDomainService nbaPointsExampleDomainService)
        {
            _nbaPointsExampleDomainService = nbaPointsExampleDomainService;
        }

        public override void Execute(GeneticOptimizeInput args)
        {
            this._nbaPointsExampleDomainService.GeneticOptimize(args).GetAwaiter().GetResult();
        }
    }
}
