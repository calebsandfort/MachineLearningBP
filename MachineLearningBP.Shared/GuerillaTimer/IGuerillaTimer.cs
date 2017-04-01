using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Shared.GuerillaTimer
{
    public interface IGuerillaTimer : ITransientDependency
    {
        void Start(String prefix);
        void Complete();
    }
}
