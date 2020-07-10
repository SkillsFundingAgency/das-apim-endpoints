using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.PassThrough;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesPassThroughService : IEmployerIncentivesPassThroughService
    {
        private readonly IPassThroughApiClient _client;

        public EmployerIncentivesPassThroughService(HttpClient httpClient)
        {
            _client = new PassThroughApiClient(httpClient);
        }

        public async Task<HealthCheckResult> HealthCheck(CancellationToken cancellationToken = default)
        {
            try
            {
                var value = await _client.GetAsync<string>("/health", cancellationToken);
                if (value == "Healthy")
                {
                    return HealthCheckResult.Healthy();
                }
                return HealthCheckResult.Unhealthy();
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }

        public Task<InnerApiResponse> AddLegalEntity(long accountId, LegalEntityRequest legalEntityRequest, CancellationToken cancellationToken = default)
        {
            return _client.PostAsync($"/accounts/{accountId}/legalentities", legalEntityRequest, true, cancellationToken);
        }

        public Task<InnerApiResponse> RemoveLegalEntity(long accountId, long accountLegalEntityId, CancellationToken cancellationToken = default)
        {
            return _client.DeleteAsync($"/accounts/{accountId}/legalentities/{accountLegalEntityId}", false, cancellationToken);
        }
    }
}
