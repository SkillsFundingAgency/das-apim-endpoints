using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Infrastructure;

public static class CorrelationContext
{
    private static readonly AsyncLocal<string> _correlationId = new();

    public static string CorrelationId
    {
        get => _correlationId.Value;
        set => _correlationId.Value = value;
    }
}

public class CorrelationTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        var correlationId = CorrelationContext.CorrelationId;

        if (!string.IsNullOrEmpty(correlationId))
        {
            telemetry.Context.GlobalProperties["x-correlation-id"] = correlationId;
        }
    }
}


public class CorrelationIdMiddleware
{
    private const string HeaderName = "x-correlation-id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        
        var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var values) &&
                            !string.IsNullOrWhiteSpace(values.FirstOrDefault())
            ? values.First()
            : Guid.NewGuid().ToString();

        CorrelationContext.CorrelationId = correlationId;
        context.Response.Headers[HeaderName] = correlationId;

        await _next(context);

    }
}
