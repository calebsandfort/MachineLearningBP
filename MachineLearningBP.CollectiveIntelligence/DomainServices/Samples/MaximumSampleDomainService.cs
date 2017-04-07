using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using Abp.Domain.Repositories;
using MachineLearningBP.CollectiveIntelligence.Entities;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Samples
{
    public class MaximumSampleDomainService<TSample, TStatLine, TTimeGrouping, TParticipant> : MediumSampleDomainService<TSample, TStatLine, TTimeGrouping>
        where TSample : Sample
        where TStatLine : StatLine
        where TTimeGrouping : TimeGrouping
        where TParticipant : Participant
    {
        #region Properties
        public readonly IRepository<TParticipant> _participantRepository;
        #endregion

        #region Constructor
        public MaximumSampleDomainService(IRepository<TParticipant> participantRepository, IRepository<TTimeGrouping> timeGroupingRepository, IRepository<TSample> sampleRepository,
            IRepository<TStatLine> statLineRepository, ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager)
            : base(timeGroupingRepository, sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager)
        {
            this._participantRepository = participantRepository;
        } 
        #endregion

    }
}
