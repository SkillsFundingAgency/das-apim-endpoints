using OpenTelemetry;

namespace SFA.DAS.Approvals.Api.Telemetry;

public class NotFoundDependencyTelemetryProcessor : BaseProcessor<System.Diagnostics.Activity>
{
    public override void OnEnd(System.Diagnostics.Activity activity)
    {
        // Suppress 404 errors for dependencies by marking them as successful
        if (activity.GetTagItem("http.status_code")?.ToString() == "404")
        {
            activity.SetTag("otel.status_code", "OK");
            activity.SetTag("otel.status_description", "Suppressed 404 error for dependency.");
        }

        // Ensure the activity is passed to the next processor
        base.OnEnd(activity);
    }
}