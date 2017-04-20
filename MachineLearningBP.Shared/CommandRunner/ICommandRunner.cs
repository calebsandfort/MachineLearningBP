using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Shared.CommandRunner
{
    public interface ICommandRunner : ISingletonDependency
    {
        void RunCmd(string executable, string cmd, string args = "");
    }
}
