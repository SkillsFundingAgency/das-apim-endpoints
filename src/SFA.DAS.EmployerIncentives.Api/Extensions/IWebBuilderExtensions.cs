using Microsoft.AspNetCore.Hosting;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerIncentives.Configuration;

namespace SFA.DAS.EmployerIncentives.Api.Extensions
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c
                .AddAzureTableStorage(EmployerIncentivesConfigurationKeys.EmployerIncentivesOuterApi,
                    EmployerIncentivesConfigurationKeys.AzureActiveDirectoryApiConfiguration)
            );
        }
    }
}
