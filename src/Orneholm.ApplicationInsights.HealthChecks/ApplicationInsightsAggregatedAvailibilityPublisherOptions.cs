namespace Orneholm.ApplicationInsights.HealthChecks
{
    public class ApplicationInsightsAggregatedAvailibilityPublisherOptions
    {
        /// <summary>
        /// Availibility telemetry name.
        /// </summary>
        public string TestName { get; set; } = "AspNetHealthChecks";

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