using MachineLearningBP.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Shared.GuerillaTimer
{
    public class GuerillaTimer : IGuerillaTimer, IDisposable
    {
        private IConsoleHubProxy _consoleHubProxy;
        public String Prefix { get; set; }
        public Stopwatch Timer { get; set; }

        #region Constructor
        public GuerillaTimer(IConsoleHubProxy consoleHubProxy, [CallerMemberName]string memberName = "")
        {
            _consoleHubProxy = consoleHubProxy;
            this.Timer = new Stopwatch();

            this.Prefix = memberName;
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"{this.Prefix} starting..."));
            this.Timer.Restart();
        }
        #endregion

        public void Dispose()
        {
            this.Timer.Stop();
            this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"{this.Prefix} completed in {this.Timer.Elapsed.ToString(@"hh\:mm\:ss")}."));
        }
    }
}
