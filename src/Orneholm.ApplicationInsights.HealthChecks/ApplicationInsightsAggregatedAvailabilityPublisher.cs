using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Orneholm.ApplicationInsights.HealthChecks
{
    public class ApplicationInsightsAggregatedAvailabilityPublisher : IHealthCheckPublisher
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly IOptions<ApplicationInsightsAggregatedAvailibilityPublisherOptions> _options;

        public ApplicationInsightsAggregatedAvailabilityPublisher(TelemetryClient telemetryClient, IOptions<ApplicationInsightsAggregatedAvailibilityPublisherOptions> options)
        {
            _telemetryClient = telemetryClient;
            _options = options;
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            var availabilityTelemetry = GetAvailabilityTelemetry(report);
            _telemetryClient.TrackAvailability(availabilityTelemetry);

            return Task.CompletedTask;
        }

        private AvailabilityTelemetry GetAvailabilityTelemetry(HealthReport report)
        {
            var availabilityTelemetry = new AvailabilityTelemetry
            {
                Timestamp = DateTimeOffset.UtcNow,

                Duration = report.TotalDuration,
                Success = GetSuccessFromStatus(report.Status),

                Name = _options.Value.TestName,
                RunLocation = _options.Value.TestRunLocation
            };

            AddHealthChecksStatuses(availabilityTelemetry, report);
            AddHealthChecksDurations(availabilityTelemetry, report);
            
            return availabilityTelemetry;
        }

        private bool GetSuccessFromStatus(HealthStatus status)
        {
            return status == HealthStatus.Healthy
                   || _options.Value.TreatDegradedAsSuccess && status == HealthStatus.Degraded;
        }

        private static void AddHealthChecksDurations(AvailabilityTelemetry telemetry, HealthReport report)
        {
            var durations = report.Entries.ToDictionary(x => $"HealthCheck-Duration-{x.Key}", x => x.Value.Duration.TotalMilliseconds);
            foreach (var duration in durations)
            {
                telemetry.Metrics.Add(duration);
            }
        }

        private static void AddHealthChecksStatuses(AvailabilityTelemetry telemetry, HealthReport report)
        {
            var statuses = report.Entries.ToDictionary(x => $"HealthCheck-Status-{x.Key}", x => x.Value.Status.ToString());
            foreach (var status in statuses)
            {
                telemetry.Properties.Add(status);
            }
        }
    }
}