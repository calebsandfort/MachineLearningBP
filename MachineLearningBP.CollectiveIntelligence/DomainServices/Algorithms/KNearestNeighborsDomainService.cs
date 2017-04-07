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
    public class KNearestNeighborsDomainService<TResult> : BaseDomainService, IKNearestNeighborsDomainService<TResult>
    {
        #region Constructor
        public KNearestNeighborsDomainService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager) : base(sqlExecuter, consoleHubProxy, settingManager)
        {
        }
        #endregion

        #region DivideData
        public void DivideData(List<Example<TResult>> data, out List<Example<TResult>> trainSet, out List<Example<TResult>> testSet, double test = .05)
        {
            List<Example<TResult>> trainSetTemp = new List<Example<TResult>>();
            List<Example<TResult>> testSetTemp = new List<Example<TResult>>();

            Random random = new Random(DateTime.Now.Millisecond);

            foreach (Example<TResult> example in data)
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
