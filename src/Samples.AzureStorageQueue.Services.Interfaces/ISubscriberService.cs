#region Using Statements
using Samples.AzureStorageQueue.Models;
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.Services.Interfaces
{
    public interface ISubscriberService
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }

        Task<bool> ProcessNextMessage();

        Task<QueueItem> PeekNextMessage();

    }
}