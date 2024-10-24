using Microsoft.ApplicationInsights.NLogTarget;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using SFA.DAS.NLog.Targets.Redis.DotNetCore;
using System.Diagnostics.CodeAnalysis;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using NLogLevel = NLog.LogLevel;

namespace SFA.DAS.Payments.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class LoggingSetup
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

[ExcludeFromCodeCoverage]
public class NLogConfiguration
{
    public void ConfigureNLog()
    {
        const string appName = "das-payments-outer-api";
        var env = Environment.GetEnvironmentVariable("EnvironmentName");
        var config = new LoggingConfiguration();

        if (string.IsNullOrEmpty(env) || env.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
        {
            AddLocalTarget(config, appName);
        }
        else
        {
            AddRedisTarget(config, appName);
            AddAppInsights(config);
        }

        LogManager.Configuration = config;
    }

    private static void AddLocalTarget(LoggingConfiguration config, string appName)
    {
        InternalLogger.LogFile = Path.Combine(Directory.GetCurrentDirectory(), $"logs\\nlog-internal.{appName}.log");
        var fileTarget = new FileTarget("Disk")
        {
            FileName = Path.Combine(Directory.GetCurrentDirectory(), $"logs\\{appName}.${{shortdate}}.log"),
            Layout = "${longdate} [${uppercase:${level}}] [${logger}] - ${message} ${onexception:${exception:format=tostring}}"
        };
        config.AddTarget(fileTarget);

        config.AddRule(GetMinLogLevel(), NLogLevel.Fatal, "Disk");
    }

    private static void AddRedisTarget(LoggingConfiguration config, string appName)
    {
        var target = new RedisTarget
        {
            Name = "RedisLog",
            AppName = appName,
            EnvironmentKeyName = "EnvironmentName",
            ConnectionStringName = "LoggingRedisConnectionString",
            IncludeAllProperties = true,
            Layout = "${message}"
        };

        config.AddTarget(target);
        config.AddRule(GetMinLogLevel(), NLogLevel.Fatal, "RedisLog");
    }

    private static void AddAppInsights(LoggingConfiguration config)
    {
        var target = new ApplicationInsightsTarget
        {
            Name = "AppInsightsLog"
        };

        config.AddTarget(target);
        config.AddRule(GetMinLogLevel(), NLogLevel.Fatal, "AppInsightsLog");
    }

    private static NLogLevel GetMinLogLevel() => NLogLevel.FromString("Info");
}
