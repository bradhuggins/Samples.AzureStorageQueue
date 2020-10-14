using Azure.Storage.Queues.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Samples.AzureStorageQueue.WebApi.Models
{
    public class SubscriberResponse
    {
        public Samples.AzureStorageQueue.Models.QueueItem Message { get; set; }

        public string ErrorMessage { get; set; }

    }

}
