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
            var availabilityTelemetry = new AvailabilityTelemetry
            {
                Timestamp = DateTimeOffset.UtcNow,

                Duration = report.TotalDuration,
                Success = GetSuccessStatus(report),

                Name = _options.Value.TestName,
                RunLocation = _options.Value.TestRunLocation
            };

            var testDurations = GetTestDurations(report);
            foreach (var testDuration in testDurations)
            {
                availabilityTelemetry.Metrics.Add(testDuration);
            }

            var testStatuses = GetTestStatuses(report);
            foreach (var testStatus in testStatuses)
            {
                availabilityTelemetry.Properties.Add(testStatus);
            }

            _telemetryClient.TrackAvailability(availabilityTelemetry);

            return Task.CompletedTask;
        }

        private static Dictionary<string, string> GetTestStatuses(HealthReport report)
        {
            return report.Entries.ToDictionary(x => $"HealthCheck-Status-{x.Key}", x => x.Value.Status.ToString());
        }

        private static Dictionary<string, double> GetTestDurations(HealthReport report)
        {
            return report.Entries.ToDictionary(x => $"HealthCheck-Duration-{x.Key}", x => x.Value.Duration.TotalMilliseconds);
        }

        private bool GetSuccessStatus(HealthReport report)
        {
            return report.Status == HealthStatus.Healthy
                   || _options.Value.TreatDegradedAsSuccess && report.Status == HealthStatus.Degraded;
        }
    }
}