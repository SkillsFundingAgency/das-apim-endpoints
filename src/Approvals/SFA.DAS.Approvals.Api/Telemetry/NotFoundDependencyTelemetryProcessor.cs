using Microsoft.Extensions.Logging;
using OpenTelemetry;
using System.Diagnostics;

namespace SFA.DAS.Approvals.Api.Telemetry;

public class NotFoundDependencyTelemetryProcessor(ILogger<NotFoundDependencyTelemetryProcessor> logger)
    : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        // Example of logging inside the processor
        logger.LogTrace("Processing dependency telemetry: {DisplayName}", activity.DisplayName);
        logger.LogTrace($"Activity kind {activity.Kind}");
        
        // Suppress 404 errors for dependencies
        if (activity.Kind == ActivityKind.Client &&
            activity.GetTagItem("http.response.status_code")?.ToString() == "404")
        {
            activity.SetStatus(ActivityStatusCode.Ok, "Suppressed 404 error for dependency.");
            logger.LogTrace("Suppressed 404 for dependency: {Target}", activity.DisplayName);
        }

        // Ensure the activity is passed to the next processor
        base.OnEnd(activity);
    }
}