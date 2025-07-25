﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
public class ApprenticeshipsApiHealthCheck : ApiHealthCheck<LearningApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Apprenticeships API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public ApprenticeshipsApiHealthCheck(ILearningApiClient<LearningApiConfiguration> client, ILogger<ApprenticeshipsApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}