#region Using Statements
using System;
#endregion

namespace Samples.AzureStorageQueue.Models
{
    public class QueueItem
    {
        public Guid Id { get; set; }

        public Guid? SourceId { get; set; }

        public DateTime Timestamp { get; set; }

        public QueueItemMesssage Message { get; set; }

        //public QueueItem()
        //{
        //    this.Id = Guid.NewGuid();
        //    this.Timestamp = DateTime.UtcNow;
        //}


    }



}
