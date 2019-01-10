namespace Orneholm.ApplicationInsights.HealthChecks
{
    public class ApplicationInsightsAggregatedAvailibilityPublisherOptions
    {
        public string TestName { get; set; } = "AspNetHealthChecks";
        public string TestRunLocation { get; set; } = "Application";
        public bool TreatDegradedAsSuccess { get; set; } = false;
    }
}