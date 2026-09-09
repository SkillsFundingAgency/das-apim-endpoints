using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.DigitalCertificates.Contracts.Client;

namespace SFA.DAS.Admin.Api.Infrastructure.HealthCheck;

public class DigitalCertificatesApiHealthCheck(
    IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> client,
    ILogger<DigitalCertificatesApiHealthCheck> logger)
    : ApiHealthCheck<DigitalCertificatesApiConfiguration>(
        HealthCheckDescription,
        HealthCheckResultDescription,
        client,
        logger), IHealthCheck
{
    public static readonly string HealthCheckDescription = "Digital Certificates API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}
