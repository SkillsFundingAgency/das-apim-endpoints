using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Interfaces;

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