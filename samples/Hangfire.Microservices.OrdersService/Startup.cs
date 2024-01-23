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
                .UseDynamicJobs()
                .UseSqlServerStorage("Database=Hangfire.DynamicJobs; Integrated Security=true; Trust Server Certificate=true"));

            services.AddHangfireServer(config => config.Queues = new [] { "orders" });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager manager)
        {
            manager.AddOrUpdateDynamic(
                "periodic-newsletter",
                () => NewsletterSender.Execute(1111),
                "* * * * *",
                new DynamicRecurringJobOptions
                {
                    Filters = new [] { new QueueAttribute("newsletter") }
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