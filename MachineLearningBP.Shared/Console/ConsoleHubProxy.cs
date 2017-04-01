﻿using Abp.Configuration;
using MachineLearningBP.Shared.Dtos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Shared
{
    public class ConsoleHubProxy : IConsoleHubProxy
    {
        private readonly ISettingManager _settingManager;

        public ConsoleHubProxy(ISettingManager settingManager)
        {
            this._settingManager = settingManager;
        }

        public void WriteLine(ConsoleWriteLineInput consoleWriteLineInput)
        {
            var client = new RestClient(this._settingManager.GetSettingValue("Path"));
            var request = new RestRequest("ConsoleSignalR/WriteLine", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { input = consoleWriteLineInput });

            RestResponse response = (RestResponse)client.Execute(request);
            //});
        }
    }
}
