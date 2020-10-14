#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; 
#endregion

namespace Samples.AzureStorageQueue.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Shared.AzureStorageQueueProxy.IService, Shared.AzureStorageQueueProxy.Service>();
            services.AddTransient<Samples.AzureStorageQueue.Services.Interfaces.IPublisherService, Samples.AzureStorageQueue.Services.Implementations.PublisherService>();
            services.AddTransient<Samples.AzureStorageQueue.Services.Interfaces.ISubscriberService, Samples.AzureStorageQueue.Services.Implementations.SubscriberService>();
            services.AddSingleton<Shared.Utilities.JsonHelper>();

            services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if(!env.IsProduction())
            {
                app.UseSwaggerDocumentation();
            }

        }
    }
}
