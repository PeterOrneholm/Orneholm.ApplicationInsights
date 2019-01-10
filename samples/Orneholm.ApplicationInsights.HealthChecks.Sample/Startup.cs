using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orneholm.ApplicationInsights.HealthChecks.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddApplicationInsightsAvailibilityPublisher()
                    .AddAsyncCheck("Sample1", async () =>
                    {
                        await Task.Delay(200);
                        return HealthCheckResult.Healthy("Sample1Result", new Dictionary<string, object>()
                        {
                            { "Key1", 1 },
                            { "Key2", "Sample" },
                        });
                    }, new List<string> { "Tag1", "Tag2", "Tag3" })
                    .AddAsyncCheck("Sample2", async () =>
                    {
                        await Task.Delay(400);
                        return HealthCheckResult.Degraded();
                    }, new List<string> { "Tag1", "Tag2" })
                    .AddAsyncCheck("Sample3", async () =>
                    {
                        await Task.Delay(800);
                        return HealthCheckResult.Unhealthy();
                    }, new List<string> { "Tag1", "Tag2" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello, World!");
            });
        }
    }
}
