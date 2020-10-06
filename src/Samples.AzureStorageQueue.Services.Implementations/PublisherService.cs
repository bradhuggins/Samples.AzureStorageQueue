#region Using Statements
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Samples.AzureStorageQueue.Services.Interfaces;
using Samples.AzureStorageQueue.Models;
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.Services.Implementations
{
    public class PublisherService : SimpleServiceBase, IPublisherService
    {
        private ILogger<PublisherService> _logger;
        private IConfiguration _configuration;
        private Shared.AzureStorageQueueProxy.IService _service;
        private string _queueName = Constants.QUEUE_NAME;

        public PublisherService(
            ILogger<PublisherService> logger, 
            IConfiguration configuration, 
            Shared.AzureStorageQueueProxy.IService service
            )
        {
            _logger = logger;
            _configuration = configuration;
            _service = service;
            _service.ConnectionString = _configuration.GetConnectionString("AzureStorageQueue");
        }

        public async Task Publish(QueueItem item)
        {
            string message = Shared.HttpUtilities.JsonHelper.Serialize(item);
            _logger.LogInformation("PUBLISH: " + message);
            var newMessage = await _service.CreateMessageAsync(_queueName, message);
            if (_service.HasError)
            {
                this.ErrorMessage = _service.ErrorMessage;                
                _logger.LogError("ERROR: " + this.ErrorMessage);
            }
            else
            {
                _logger.LogInformation("MessageId: " + newMessage.MessageId);
            }
        }



    }
}
