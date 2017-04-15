using MachineLearningBP.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MachineLearningBP.Shared.Framework;

namespace MachineLearningBP.Shared.GuerillaTimer
{
    public class GuerillaTimer : IGuerillaTimer, IDisposable
    {
        private IConsoleHubProxy _consoleHubProxy;
        public String Prefix { get; set; }
        public Stopwatch Timer { get; set; }
        public bool Show { get; set; }

        #region Constructor
        public GuerillaTimer(IConsoleHubProxy consoleHubProxy, String prefix = "", bool show = true, [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "")
        {
            _consoleHubProxy = consoleHubProxy;
            this.Timer = new Stopwatch();

            this.Prefix = String.IsNullOrEmpty(prefix) ? Extensions.CombineFileAndMember(filePath, memberName) : prefix;
            this.Show = show;

            if(this.Show)
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"{this.Prefix} starting..."));
            this.Timer.Restart();

        }
        #endregion

        public void Dispose()
        {
            this.Timer.Stop();

            if (this.Show)
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"{this.Prefix} completed in {this.Timer.Elapsed.ToString(@"hh\:mm\:ss\:fff")}."));
        }
    }
}
