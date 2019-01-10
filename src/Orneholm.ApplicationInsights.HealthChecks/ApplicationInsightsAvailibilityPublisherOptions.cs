namespace Orneholm.ApplicationInsights.HealthChecks
{
    public class ApplicationInsightsAvailibilityPublisherOptions
    {
        public string TestNamePrefix { get; set; } = "AspNetHealthCheck-";
        public string TestRunLocation { get; set; } = "Application";
        public bool TreatDegradedAsSuccess { get; set; } = false;
    }
}