﻿using System.Diagnostics.CodeAnalysis;
using NLog.Extensions.Logging;

namespace SFA.DAS.Earnings.Api;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNLog(this IServiceCollection serviceCollection)
    {
        var nLogConfiguration = new NLogConfiguration();

        serviceCollection.AddLogging((options) =>
        {
            options.AddFilter(typeof(Startup).Namespace, LogLevel.Information);
            options.SetMinimumLevel(LogLevel.Trace);
            options.AddNLog(new NLogProviderOptions
            {
                CaptureMessageTemplates = true,
                CaptureMessageProperties = true
            });
            options.AddConsole();

            nLogConfiguration.ConfigureNLog();
        });

        return serviceCollection;
    }
}