using Microsoft.ApplicationInsights;

namespace SFA.DAS.LearnerData.Api.Middleware
{
    public class ConcurrencyTrackingMiddleware
    {
        private static int _currentRequests = 0;
        private readonly RequestDelegate _next;
        private readonly TelemetryClient _telemetry;

        public ConcurrencyTrackingMiddleware(RequestDelegate next, TelemetryClient telemetry)
        {
            _next = next;
            _telemetry = telemetry;
        }

        public async Task Invoke(HttpContext context)
        {
            Interlocked.Increment(ref _currentRequests);

            _telemetry.TrackMetric("CurrentConcurrentRequests", _currentRequests);

            try
            {
                await _next(context);
            }
            finally
            {
                Interlocked.Decrement(ref _currentRequests);
            }
        }
    }
}
