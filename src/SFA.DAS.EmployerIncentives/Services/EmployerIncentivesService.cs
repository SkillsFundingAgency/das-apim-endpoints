using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.Commitments;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesService : IEmployerIncentivesService
    {
        private readonly IRestApiClient _client;

        public EmployerIncentivesService(HttpClient httpClient)
        {
            _client = new RestApiClient(httpClient);
        }

        public async Task<HealthCheckResult> HealthCheck(CancellationToken cancellationToken = default)
        {
            try
            {
                var value = await _client.Get("/health", null, cancellationToken);
                switch (value)
                {
                    case "Healthy":
                        return HealthCheckResult.Healthy();
                    case "Degraded":
                        return HealthCheckResult.Degraded();
                    default:
                        return HealthCheckResult.Unhealthy();
                }
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }

        public async Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship, CancellationToken cancellationToken = default)
        {
            ConcurrentBag<ApprenticeshipItem> bag = new ConcurrentBag<ApprenticeshipItem>();
            var tasks = allApprenticeship.Select(x => VerifyApprenticeshipIsEligible(x, bag, cancellationToken));
            await Task.WhenAll(tasks);

            return bag.ToArray();
        }

        private async Task VerifyApprenticeshipIsEligible(ApprenticeshipItem apprenticeship, ConcurrentBag<ApprenticeshipItem> bag, CancellationToken cancellationToken)
        {
            var statusCode = await _client.GetHttpStatusCode($"eligible-apprenticeships/{apprenticeship.Uln}", new { apprenticeship.StartDate, IsApproved = true }, cancellationToken);
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    bag.Add(apprenticeship);
                    break;
                case HttpStatusCode.NotFound:
                    break;
                default:
                    throw new ApplicationException($"Unable to get status for apprentice Uln {apprenticeship.Uln}");
            }
        }
    }
}
