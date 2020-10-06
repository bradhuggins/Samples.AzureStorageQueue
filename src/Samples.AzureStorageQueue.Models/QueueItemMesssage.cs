#region Using Statements
using System.Collections.Generic;
#endregion

namespace Samples.AzureStorageQueue.Models
{
    public class QueueItemMesssage
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public Dictionary<string,string> AdditionalProperties { get; set; }


    }
}