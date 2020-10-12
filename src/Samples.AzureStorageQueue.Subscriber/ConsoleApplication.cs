#region Using Statements
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Samples.AzureStorageQueue.Services.Interfaces;
using System;
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.Subscriber
{
    public class ConsoleApplication
    {
        readonly ILogger<ConsoleApplication> _logger;
        readonly IConfiguration _configuration;
        readonly ISubscriberService _service;

        public string ErrorMessage { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

        public ConsoleApplication(
            ILogger<ConsoleApplication> logger,
            IConfiguration configuration,
            ISubscriberService service
            )
        {
            _logger = logger;
            _configuration = configuration;
            _service = service;
        }

        public async Task Run()
        {
            //do work here (call a service)
            bool neverTrue = false;
            do
            {
                int wait = new Random().Next(500, _configuration.GetValue<int>("BasicDelayMaxMs"));
                string startMessage = $"Executing Run in {_configuration.GetValue<string>("Environment")} then waiting for {wait.ToString()} ms.";
                _logger.LogInformation(startMessage);

                await _service.ProcessNextMessage();

                System.Threading.Thread.Sleep(wait);
            }
            while (neverTrue == false);

            _logger.LogInformation("Run Completed.");
        }


    }
}
