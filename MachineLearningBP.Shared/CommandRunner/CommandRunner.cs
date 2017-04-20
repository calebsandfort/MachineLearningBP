using MachineLearningBP.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Shared.CommandRunner
{
    public class CommandRunner : ICommandRunner
    {
        readonly IConsoleHubProxy _consoleHubProxy;

        #region Constructor
        public CommandRunner(IConsoleHubProxy consoleHubProxy)
        {
            this._consoleHubProxy = consoleHubProxy;
        }
        #endregion

        #region RunCmd
        public void RunCmd(string executable, string cmd, string args = "")
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = $"{executable}.exe";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                    this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create(result));
                }
            }
        } 
        #endregion
    }
}
