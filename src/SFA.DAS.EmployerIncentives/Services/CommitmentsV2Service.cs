using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.Commitments;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class CommitmentsV2Service : ICommitmentsV2Service
    {
        private readonly IRestApiClient _restApiClient;

        public CommitmentsV2Service(HttpClient client)
        {
            _restApiClient = new RestApiClient(client);
        }

        public async Task<HealthCheckResult> HealthCheck(CancellationToken cancellationToken = default)
        {
            try
            {
                var health = await _restApiClient.Get<HealthResponse>("/health",null, cancellationToken);

                if (health == null)
                {
                    return HealthCheckResult.Unhealthy();
                }

                switch (health.Status)
                {
                    case "Healthy" :
                        return HealthCheckResult.Healthy();
                    case "Degraded" :
                        return HealthCheckResult.Degraded();
                    default :
                        return HealthCheckResult.Unhealthy();
                }
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }

        public async Task<IEnumerable<ApprenticeshipItem>> Apprenticeships(long accountId, long accountLegalEntityId,
            CancellationToken cancellationToken = default)
        {
            var response = await _restApiClient.Get<ApprenticeshipSearchResponse>("api/apprenticeships",
                new {accountId, accountLegalEntityId}, cancellationToken);

            return response.Apprenticeships;
        }
    }
}
