﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class ApprenticeCommitmentsApiHealthCheck : ApiHealthCheck<ApprenticeCommitmentsApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Apprentice Commitments API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public ApprenticeCommitmentsApiHealthCheck(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> client, ILogger<ApprenticeCommitmentsApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}