using System;
using Hangfire.Microservices.NewsletterService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hangfire.Microservices.OrdersService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDynamicJobs()
                .UseSqlServerStorage("Database=Hangfire.DynamicJobs; Integrated Security=true; Trust Server Certificate=true"));

            services.AddHangfireServer(config => config.Queues = new [] { "orders" });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager manager)
        {
            var campaignId = 1111;

            manager.AddOrUpdateDynamic(
                "periodic-newsletter",
                () => NewsletterSender.Execute(campaignId),
                "* * * * *",
                new DynamicRecurringJobOptions
                {
                    Filters = new [] { new QueueAttribute("newsletter") },
                    DisplayName = $"Process newsletter '{campaignId}'"
                });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World from Orders Service!"); });
            });
        }
    }
}