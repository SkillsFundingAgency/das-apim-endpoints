using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminAan.Api.HealthCheck;

public class EducationalOrganisationApiHealthCheck(
    IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> apiClient,
    ILogger<EducationalOrganisationApiHealthCheck> logger)
    : ApiHealthCheck<EducationalOrganisationApiConfiguration>(HealthCheckDescription, HealthCheckResultDescription,
        apiClient, logger), IHealthCheck
{
    private static readonly string HealthCheckDescription = "Educational Organisations API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}