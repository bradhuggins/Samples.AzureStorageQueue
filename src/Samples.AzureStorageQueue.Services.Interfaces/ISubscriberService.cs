#region Using Statements
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.Services.Interfaces
{
    public interface ISubscriberService
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }

        Task<bool> ProcessNextMessage();

    }
}