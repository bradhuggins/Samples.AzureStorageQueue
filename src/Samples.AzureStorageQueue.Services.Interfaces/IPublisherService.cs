using Samples.AzureStorageQueue.Models;
using System.Threading.Tasks;

namespace Samples.AzureStorageQueue.Services.Interfaces
{
    public interface IPublisherService
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }

        Task Publish(QueueItem item);
    }
}