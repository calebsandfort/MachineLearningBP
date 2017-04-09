using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms
{
    public class KNearestNeighborsDomainService<TExample, TStatLine, TResult> : BaseDomainService, IKNearestNeighborsDomainService<TExample, TStatLine, TResult>
        where TExample : Example<TStatLine, TResult>
        where TStatLine : StatLine
    {
        #region Constructor
        public KNearestNeighborsDomainService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager) : base(sqlExecuter, consoleHubProxy, settingManager)
        {
        }
        #endregion

        #region DivideData
        public void DivideData(List<TExample> data, out List<TExample> trainSet, out List<TExample> testSet, double test = .05)
        {
            List<TExample> trainSetTemp = new List<TExample>();
            List<TExample> testSetTemp = new List<TExample>();

            Random random = new Random(DateTime.Now.Millisecond);

            foreach (TExample example in data)
            {
                if (random.NextDouble() < test)
                    testSetTemp.Add(example);
                else
                    trainSetTemp.Add(example);
            }

            trainSet = trainSetTemp;
            testSet = testSetTemp;
        } 
        #endregion
    }
}
