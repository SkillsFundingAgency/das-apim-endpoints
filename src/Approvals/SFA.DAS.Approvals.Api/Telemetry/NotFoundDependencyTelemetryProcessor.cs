using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace SFA.DAS.Approvals.Api.Telemetry;

/// <summary>
/// Telemetry Processor that prevents 404 Not Found results being logged as errors
/// </summary>
public class NotFoundDependencyTelemetryProcessor(ITelemetryProcessor next) : ITelemetryProcessor
{
    private ITelemetryProcessor Next { get; set; } = next;

    /// <summary>
    /// Process a collected telemetry item.
    /// </summary>
    public void Process(ITelemetry item)
    {
        if (item is DependencyTelemetry { ResultCode: "404" } dependency)
        {
            dependency.Success = true;
        }

        Next.Process(item);
    }
}