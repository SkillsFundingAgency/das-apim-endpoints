﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
public class RoatpApiHealthCheck : ApiHealthCheck<RoatpConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Roatp API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public RoatpApiHealthCheck(IRoatpServiceApiClient<RoatpConfiguration> client, ILogger<RoatpApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}
