#region Using Statements

#endregion

namespace Samples.AzureStorageQueue.Services.Implementations
{
    public abstract class SimpleServiceBase
    {
        public string ErrorMessage { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

    }
}
