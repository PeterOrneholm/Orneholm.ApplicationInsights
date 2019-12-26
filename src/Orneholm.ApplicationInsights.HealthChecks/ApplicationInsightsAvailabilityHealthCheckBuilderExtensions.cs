using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orneholm.ApplicationInsights.HealthChecks
{
    public static class ApplicationInsightsAvailabilityHealthCheckBuilderExtensions
    {
        /// <summary>
        /// Publish each health check report as individual availability telemetry and send it to Application Insights. Contains more details such as data returned from each check.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>to use.</param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddApplicationInsightsAvailabilityPublisher(this IHealthChecksBuilder builder)
        {
            return AddApplicationInsightsAvailabilityPublisher(builder, null);
        }

        /// <summary>
        /// Publish each health check report as individual availability telemetry and send it to Application Insights. Contains more details such as data returned from each check.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>to use.</param>
        /// <param name="options">Options that can bu customized.</param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddApplicationInsightsAvailabilityPublisher(this IHealthChecksBuilder builder, Action<ApplicationInsightsAvailibilityPublisherOptions>? options)
        {
            if (options != null)
            {
                builder.Services.Configure(options);
            }

            builder.Services.AddSingleton<IHealthCheckPublisher, ApplicationInsightsAvailabilityPublisher>();

            return builder;
        }


        /// <summary>
        /// Aggregate all health check reports into one availability telemetry and send it to Application Insights. Only contains general details and details on status, duration and description for each health check.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>to use.</param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddApplicationInsightsAggregatedAvailabilityPublisher(this IHealthChecksBuilder builder)
        {
            return AddApplicationInsightsAggregatedAvailabilityPublisher(builder, null);
        }

        /// <summary>
        /// Aggregate all health check reports into one availability telemetry and send it to Application Insights. Only contains general details and details on status, duration and description for each health check.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>to use.</param>
        /// <param name="options">Options that can bu customized.</param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddApplicationInsightsAggregatedAvailabilityPublisher(this IHealthChecksBuilder builder, Action<ApplicationInsightsAggregatedAvailibilityPublisherOptions>? options)
        {
            if (options != null)
            {
                builder.Services.Configure(options);
            }

            builder.Services.AddSingleton<IHealthCheckPublisher, ApplicationInsightsAggregatedAvailabilityPublisher>();

            return builder;
        }
    }
}
