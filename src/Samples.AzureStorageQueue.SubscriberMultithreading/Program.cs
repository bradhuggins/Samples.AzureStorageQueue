﻿#region Using Statements
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.SubscriberMultithreading
{
    class Program
    {
        private static IConfiguration _configuration;
        private static ServiceProvider _serviceProvider;
        public static IServiceScope ServiceScope;

        async static Task Main(string[] args)
        {

            RegisterServices();
            ServiceScope = _serviceProvider.CreateScope();
            var cmd = ServiceScope.ServiceProvider.GetRequiredService<ConsoleApplication>();
            cmd.Run();
            if (cmd.HasError)
            {
                Console.WriteLine("ERROR: " + cmd.ErrorMessage);
                Console.ReadKey();
            }
            DisposeServices();
        }

        private static void RegisterServices()
        {
            IServiceCollection services = new ServiceCollection();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            services.AddLogging(config =>
            {
                // clear out default configuration
                config.ClearProviders();

                config.AddConfiguration(_configuration.GetSection("Logging"));
                config.AddDebug();
                config.AddConsole();
                //config.AddApplicationInsights();
            });

            services.AddSingleton(_configuration);
            services.AddSingleton<ConsoleApplication>();
            services.AddTransient<Shared.AzureStorageQueueProxy.IService, Shared.AzureStorageQueueProxy.Service>();
            //services.AddTransient<Samples.AzureStorageQueue.Services.Interfaces.IPublisherService, Samples.AzureStorageQueue.Services.Implementations.PublisherService>();
            services.AddTransient<Samples.AzureStorageQueue.Services.Interfaces.ISubscriberService, Samples.AzureStorageQueue.Services.Implementations.SubscriberService>();
            services.AddSingleton<Shared.HttpUtilities.JsonHelper>();
            services.AddTransient<ISecondaryAppThread, SecondaryAppThread>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
