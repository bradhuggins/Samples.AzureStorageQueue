#region Using Statements
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Samples.AzureStorageQueue.Models;
using Samples.AzureStorageQueue.Services.Interfaces;
using System;
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.Services.Implementations
{
    public class SubscriberService : SimpleServiceBase, ISubscriberService
    {

        private readonly ILogger<SubscriberService> _logger;
        private readonly IConfiguration _configuration;
        private readonly Shared.AzureStorageQueueProxy.IService _service;
        private readonly string _queueName = Constants.QUEUE_NAME;


        public SubscriberService(
            ILogger<SubscriberService> logger,
            IConfiguration configuration,
            Shared.AzureStorageQueueProxy.IService service
            )
        {
            _logger = logger;
            _configuration = configuration;
            _service = service;
            _service.ConnectionString = _configuration.GetConnectionString("AzureStorageQueue");
        }


        public async Task<bool> ProcessNextMessage()
        {
            bool toReturn = true;
            var message = await _service.ReadMessageAsync(_queueName);
            if (_service.HasError)
            {
                this.ErrorMessage = _service.ErrorMessage;
                _logger.LogError("ERROR: " + this.ErrorMessage);
            }
            else
            {
                if (message != null)
                {
                    _logger.LogInformation("SUBSCRIBE: " + message);

                    //parse message and do some processing
                    QueueItem item = await Shared.Utilities.JsonHelper.Deserialize<QueueItem>(Utilities.ToBase64DecodedString(message.MessageText));
                    string output = $"ID: {item.Id.ToString()}";
                    if (item.Message.AdditionalProperties != null)
                    {
                        string source = string.Empty;
                        if (item.Message.AdditionalProperties.TryGetValue("source", out source))
                        {
                            output += $" | Source: {source}";
                        }
                        string foreignKey = string.Empty;
                        if (item.Message.AdditionalProperties.TryGetValue("foreignKey", out foreignKey))
                        {
                            output += $" | ForeignKey: {foreignKey}";
                        }
                    }
                    _logger.LogInformation("OUPUT: " + output);

                    //dequeue or not
                    int op1 = new Random().Next(0, 10);
                    if (op1 == 0)
                    {
                        //simulate failure.
                        _logger.LogWarning("WARNING: Simulate error and reset message.");
                    }
                    else
                    {
                        await _service.DeleteMessageAsync(_queueName, message.MessageId, message.PopReceipt);
                    }
                }
                else
                {
                    _logger.LogInformation("No messages found.");
                    toReturn = false;
                }
            }
            return toReturn;

        }

        public async Task<QueueItem> PeekNextMessage()
        {
            QueueItem toReturn = null;
            var message = await _service.ReadMessageAsync(_queueName);
            if (_service.HasError)
            {
                this.ErrorMessage = _service.ErrorMessage;
                _logger.LogError("ERROR: " + this.ErrorMessage);
            }
            else
            {
                if (message != null)
                {
                    _logger.LogInformation("SUBSCRIBE: " + message);

                    //parse message and do some processing
                    toReturn = await Shared.Utilities.JsonHelper.Deserialize<QueueItem>(message.MessageText);                    
                }
                else
                {
                    _logger.LogInformation("No messages found.");
                }
            }
            return toReturn;

        }
    }


}
