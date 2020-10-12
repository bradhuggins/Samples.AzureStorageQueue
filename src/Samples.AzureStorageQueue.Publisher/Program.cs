﻿#region Using Statements
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
#endregion

namespace Samples.AzureStorageQueue.Publisher
{
    class Program
    {
        private static ServiceProvider _serviceProvider;
        private static IConfiguration _configuration;


        async static Task Main(string[] args)
        {

            RegisterServices();
            IServiceScope serviceScope = _serviceProvider.CreateScope();
            var cmd = serviceScope.ServiceProvider.GetRequiredService<ConsoleApplication>();
            await cmd.Run();
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
            services.AddTransient<Samples.AzureStorageQueue.Services.Interfaces.IPublisherService, Samples.AzureStorageQueue.Services.Implementations.PublisherService>();
            //services.AddTransient<Samples.AzureStorageQueue.Services.ISubscriberService, Samples.AzureStorageQueue.Services.SubscriberService>();
            services.AddSingleton<Shared.Utilities.JsonHelper>();
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
