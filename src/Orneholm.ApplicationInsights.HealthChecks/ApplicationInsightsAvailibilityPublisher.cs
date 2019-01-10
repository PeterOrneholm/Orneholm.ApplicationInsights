using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Orneholm.ApplicationInsights.HealthChecks
{
    public class ApplicationInsightsAvailibilityPublisher : IHealthCheckPublisher
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsAvailibilityPublisher(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}