using MachineLearningBP.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Shared.GuerillaTimer
{
    public class GuerillaTimer : IGuerillaTimer
    {
        private IConsoleHubProxy _consoleHubProxy;
        public String Prefix { get; set; }
        public Stopwatch Timer { get; set; }

        #region Constructor
        public GuerillaTimer(IConsoleHubProxy consoleHubProxy)
        {
            _consoleHubProxy = consoleHubProxy;
            this.Timer = new Stopwatch();
        }
        #endregion

        public void Start(string prefix)
        {
            this.Prefix = prefix;
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"{this.Prefix} starting..."));
            this.Timer.Restart();
        }

        public void Complete()
        {
            this.Timer.Stop();
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"{this.Prefix} completed in {this.Timer.Elapsed.ToString(@"hh\:mm\:ss")}."));
        }
    }
}
