namespace Orneholm.ApplicationInsights.HealthChecks
{
    public class ApplicationInsightsAvailibilityPublisherOptions
    {
        /// <summary>
        /// Prefix for availibility telemetry name.
        /// </summary>
        public string TestNamePrefix { get; set; } = "AspNetHealthCheck-";

        /// <summary>
        /// Availibility telemetry run location.
        /// </summary>
        public string TestRunLocation { get; set; } = "Application";

        /// <summary>
        /// If a degredes status indicates that the Availibility telemetry is not successful.
        /// </summary>
        public bool TreatDegradedAsSuccess { get; set; } = false;
    }
}