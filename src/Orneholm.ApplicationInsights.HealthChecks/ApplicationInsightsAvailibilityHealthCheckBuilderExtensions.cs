using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orneholm.ApplicationInsights.HealthChecks
{
    public static class ApplicationInsightsAvailibilityHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddApplicationInsightsAvailibilityPublisher(this IHealthChecksBuilder builder, string instrumentationKey = null)
        {
            builder.Services.AddSingleton<IHealthCheckPublisher, ApplicationInsightsAvailibilityPublisher>();
            return builder;
        }
    }
}
