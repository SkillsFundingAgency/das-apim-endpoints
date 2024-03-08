﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Api.Common.Infrastructure;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class HealthCheckStartup
{
    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
        });

        app.UseHealthChecks("/ping", new HealthCheckOptions
        {
            Predicate = (_) => false,
            ResponseWriter = (context, report) =>
            {
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("");
            }
        });

        return app;
    }
}
