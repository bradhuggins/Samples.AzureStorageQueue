﻿namespace Samples.AzureStorageQueue.SubscriberMultithreading
{
    public interface ISecondaryAppThread
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }
        int Id { get; set; }

        event SecondaryAppThreadTerminatingEventHandler Terminating;

        void Execute();
    }
}
