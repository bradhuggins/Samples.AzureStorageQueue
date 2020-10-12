#region Using Statements
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Samples.AzureStorageQueue.Services.Implementations;
using Samples.AzureStorageQueue.Services.Interfaces;
using System;
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.Publisher
{
    public class ConsoleApplication
    {
        readonly ILogger<ConsoleApplication> _logger;
        readonly IConfiguration _configuration;
        readonly IPublisherService _service;

        public string ErrorMessage { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

        public ConsoleApplication(
            ILogger<ConsoleApplication> logger,
            IConfiguration configuration,
            IPublisherService service
            )
        {
            _logger = logger;
            _configuration = configuration;
            _service = service;
        }

        public async Task Run()
        {
            //do work here (call a service)
            int x = 0;
            do
            {

                int wait = new Random().Next(500, _configuration.GetValue<int>("BasicDelayMaxMs"));
                string startMessage = $"Executing Run in {_configuration.GetValue<string>("Environment")}. Waiting for {wait.ToString()} ms.";
                _logger.LogInformation(startMessage);

                var item = Utilities.GetQueueItem();
                await _service.Publish(item);

                System.Threading.Thread.Sleep(wait);
            }
            while (x == 0);

            _logger.LogInformation("Run Completed.");
        }


    }
}
