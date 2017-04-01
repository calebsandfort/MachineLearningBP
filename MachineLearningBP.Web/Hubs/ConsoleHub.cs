using Abp.Dependency;
using MachineLearningBP.Shared.Dtos;
using Microsoft.AspNet.SignalR;

namespace MachineLearningBP.Web.Hubs
{
    public class ConsoleHub : Hub, ISingletonDependency
    {
        public void WriteLine(ConsoleWriteLineInput input)
        {
            Clients.All.writeLine(input.Line);
        }
    }
}