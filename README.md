# Orneholm - Application Insights - Health Checks

[![License: MIT](https://img.shields.io/badge/License-MIT-orange.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://dev.azure.com/orneholm/Orneholm.ApplicationInsights/_apis/build/status/Orneholm.ApplicationInsights?branchName=master)](https://dev.azure.com/orneholm/Orneholm.ApplicationInsights/_build/latest?definitionId=5&branchName=master)
[![NuGet](https://img.shields.io/nuget/v/Orneholm.ApplicationInsights.HealthChecks.svg)](https://www.nuget.org/packages/Orneholm.ApplicationInsights.HealthChecks/)
[![Twitter Follow](https://img.shields.io/badge/Twitter-@PeterOrneholm-blue.svg?logo=twitter)](https://twitter.com/PeterOrneholm)

ASP.NET Core Health check publisher for Azure Application Insights that will publish the health check reports as availibility telemetry, including details and data.
As described in the [Microsoft Docs](https://docs.microsoft.com/en-us/azure/azure-monitor/app/monitor-web-app-availability#alert-on-custom-analytics-queries) custom availibility telemetry does not show up in all dashboards, but you can easily trigger alerts on them and show data by custom Log Analytics queries.

Please read the announcement blogpost for some backstory and a few images:
https://peter.orneholm.com/post/181922490118/introducing-orneholmapplicationinsightshealthchec

## Install the NuGet package

Orneholm.ApplicationInsights.HealthChecks is distributed as [packages on NuGet](https://www.nuget.org/profiles/PeterOrneholm), install using the tool of your choice, for example _dotnet cli_.

```console
dotnet add package ApplicationInsights.HealthChecks
```

## Getting started

To get started, configure your ASP.NET Core app to use Application Insights:

```csharp
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights()
            .UseStartup<Startup>();
```

Then, in your `Startup.cs`, where you register your services, add the Application Insights Availability Publisher:

```csharp
services.AddHealthChecks()
        .AddCheck("SampleCheck1", () => HealthCheckResult.Healthy())
        .AddCheck("SampleCheck2", () => HealthCheckResult.Degraded())
        .AddCheck("SampleCheck3", () => HealthCheckResult.Unhealthy())
        .AddApplicationInsightsAggregatedAvailabilityPublisher()
        .AddApplicationInsightsAvailabilityPublisher();
```

- `.AddApplicationInsightsAggregatedAvailabilityPublisher()`: Aggregate all health check reports into one availability telemetry and send it to Application Insights. Only contains general details and details on status, duration and description for each health check.
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
### Troubleshooting
If you are running ASP.NET Core 2.2 and have an issue where the health check results are not sent to Application Insights you might be affected by a [bug](https://github.com/aspnet/Extensions/issues/639).
The workaround is to add the following line after you call services.AddHealthChecks() in ConfigureServices

```csharp
services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), typeof(HealthCheckPublisherOptions).Assembly.GetType("Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckPublisherHostedService")));
```

---

## Samples & Test

For more use cases, samples and inspiration; feel free to browse our sample.

- [Orneholm.ApplicationInsights.HealthChecks.Sample](samples/Orneholm.ApplicationInsights.HealthChecks.Sample)

## Contribute

We are very open to community contributions.
Please see our [contribution guidelines](CONTRIBUTING.md) before getting started.

### License & acknowledgements

Orneholm.PEAccountingNet is licensed under the very permissive [MIT license](https://opensource.org/licenses/MIT) for you to be able to use it in commercial or non-commercial applications without many restrictions.

The brand Application Insights belongs to Microsoft.
