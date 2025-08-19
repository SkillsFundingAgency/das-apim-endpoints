using Microsoft.Extensions.Logging.ApplicationInsights;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Apprenticeships.Api;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogging(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddLogging(builder =>
        {
            builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
        });

        return serviceCollection;
    }
}