#region Using Statements
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Samples.AzureStorageQueue.Services.Interfaces;
using System;
#endregion

namespace Samples.AzureStorageQueue.SubscriberMultithreading
{
    public class SecondaryAppThread : ISecondaryAppThread
    {
        readonly ILogger<SecondaryAppThread> _logger;
        readonly IConfiguration _configuration;
        readonly ISubscriberService _service;

        public int Id { get; set; }

        public event SecondaryAppThreadTerminatingEventHandler Terminating;
        public string ErrorMessage { get; set; }
        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

        public SecondaryAppThread(
            ILogger<SecondaryAppThread> logger,
            IConfiguration configuration,
            ISubscriberService service
            )
        {
            _logger = logger;
            _configuration = configuration;
            _service = service;
        }

        public void Execute()
        {
            try
            {
                bool result = true;
                int wait = new Random().Next(500, _configuration.GetValue<int>("BasicDelayMaxMs"));
                string startMessage = $"Thread Id {this.Id} executing then waiting for {wait.ToString()} ms.";
                _logger.LogInformation(startMessage);

                result = _service.ProcessNextMessage().Result;

                System.Threading.Thread.Sleep(wait);
            }
            finally
            {
                this.Terminating(this.Id);
            }
            return;
        }
    }
}
