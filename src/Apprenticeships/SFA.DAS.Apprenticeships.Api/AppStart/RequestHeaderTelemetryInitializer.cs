using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace SFA.DAS.Apprenticeships.Api.AppStart;

public class RequestHeaderTelemetryInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestHeaderTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        var requestTelemetry = telemetry as RequestTelemetry;
        if (requestTelemetry == null) return;

        var context = _httpContextAccessor.HttpContext;
        if (context == null) return;

        var requestHeaders = context.Request.Headers.Keys;
        var responseHeaders = context.Response.Headers.Keys;
        foreach (var headerName in requestHeaders)
        {
            var headers = context.Request.Headers[headerName];
            if (headers.Any())
            {
                telemetry.Context.GlobalProperties.Add($"Request_{headerName}", string.Join(Environment.NewLine, headers));
            }             
        }
        foreach (var headerName in responseHeaders)
        {
            var headers = context.Response.Headers[headerName];
            if (headers.Any())
            {
                telemetry.Context.GlobalProperties.Add($"Response_{headerName}", string.Join(Environment.NewLine, headers));
            }
        }
    }
}