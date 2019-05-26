using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Orneholm.ApplicationInsights.HealthChecks
{
    public class ApplicationInsightsAvailabilityPublisher : IHealthCheckPublisher
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly IOptions<ApplicationInsightsAvailibilityPublisherOptions> _options;

        public ApplicationInsightsAvailabilityPublisher(TelemetryClient telemetryClient, IOptions<ApplicationInsightsAvailibilityPublisherOptions> options)
        {
            _telemetryClient = telemetryClient;
            _options = options;
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            foreach (var entry in report.Entries)
            {
                var availabilityTelemetry = GetAvailabilityTelemetry(entry.Key, entry.Value);
                _telemetryClient.TrackAvailability(availabilityTelemetry);
            }

            return Task.CompletedTask;
        }

        private AvailabilityTelemetry GetAvailabilityTelemetry(string name, HealthReportEntry entry)
        {
            var availabilityTelemetry = new AvailabilityTelemetry
            {
                Timestamp = DateTimeOffset.UtcNow,

                Duration = entry.Duration,
                Success = GetSuccessFromStatus(entry.Status),

                Name = $"{_options.Value.TestNamePrefix}{name}",
                RunLocation = _options.Value.TestRunLocation,
                Message = entry.Description
            };

            AddHealthReportEntryData(availabilityTelemetry, entry);

            return availabilityTelemetry;
        }

        private bool GetSuccessFromStatus(HealthStatus status)
        {
            return status == HealthStatus.Healthy
                   || _options.Value.TreatDegradedAsSuccess && status == HealthStatus.Degraded;
        }

        private static void AddHealthReportEntryData(AvailabilityTelemetry telemetry, HealthReportEntry entry)
        {
            foreach (var data in entry.Data)
            {
                telemetry.Properties.Add($"HealthCheck-Data-{data.Key}", data.Value?.ToString()?? "<null>");
            }
        }
    }
}