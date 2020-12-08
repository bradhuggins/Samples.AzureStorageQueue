#region Using Statements
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Samples.AzureStorageQueue.Models;
using Samples.AzureStorageQueue.Services.Interfaces;
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.Services.Implementations
{
    public class PublisherService : SimpleServiceBase, IPublisherService
    {
        private readonly ILogger<PublisherService> _logger;
        private readonly IConfiguration _configuration;
        private readonly Shared.AzureStorageQueueProxy.IService _service;
        private readonly string _queueName = Constants.QUEUE_NAME;

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
            string jsonMsg = Shared.Utilities.JsonHelper.Serialize(item);
            string message = Utilities.ToBase64EncodedString(jsonMsg);
            _logger.LogInformation("PUBLISH: " + jsonMsg);
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
