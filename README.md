# Orneholm - Application Insights - Health Checks

ASP.NET Core Health check publisher for Azure Application Insights that will puslibsh the health check reports as availibility results, including details and data.

## Install

Orneholm.ApplicationInsights.HealthChecks is distributed as [package on NuGet](https://www.nuget.org/packages/Orneholm.ApplicationInsights.HealthChecks/), install using the tool of your choice, for example _dotnet cli_:

```powershell
dotnet add package Orneholm.ApplicationInsights.HealthChecks
```

## Getting started

To get started, configure your ASP.NET Core 2.2 app to use Application Insights:

```csharp
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights()
            .UseStartup<Startup>();
```

Then, in your `Startup.cs`, where you register your services, add the Application Insights  Availability Publisher:

```csharp
services.AddHealthChecks()
        .AddCheck("SampleCheck1", () => HealthCheckResult.Healthy())
		.AddCheck("SampleCheck2", () => HealthCheckResult.Degraded())
		.AddCheck("SampleCheck3", () => HealthCheckResult.Unhealthy())
        .AddApplicationInsightsAggregatedAvailabilityPublisher()
        .AddApplicationInsightsAvailabilityPublisher();
```

- `.AddApplicationInsightsAggregatedAvailabilityPublisher()`: This will aggregate all health check reports into one availability telemetry and send it to Application Insights. Only contains general details and detyails on status, duration and description for each check.
- `.AddApplicationInsightsAvailabilityPublisher()`: This will publish each health check report as individual availability telemetry and send it to Application Insights. Contains more details such as data returned from each check.

Both of them can be used side by side, to get one item for the general health of the system and then multiple ones with more details.

## Customization

You can customize some options, by using an overload:

```csharp
.AddApplicationInsightsAggregatedAvailabilityPublisher(options =>
{
    options.TestName = "AspNetHealthChecks";
    options.TestRunLocation = "Application";
    options.TreatDegradedAsSuccess = false;
})
.AddApplicationInsightsAvailabilityPublisher(options =>
{
    options.TestNamePrefix = "AspNetHealthCheck";
    options.TestRunLocation = "Application";
    options.TreatDegradedAsSuccess = false;
});
```