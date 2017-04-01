using Abp.Dependency;
using MachineLearningBP.Shared.Dtos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Shared
{
    public interface IConsoleHubProxy : ISingletonDependency
    {
        void WriteLine(ConsoleWriteLineInput consoleWriteLineInput);
    }
}
