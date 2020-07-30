using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.Commitments;
using SFA.DAS.EmployerIncentives.Models.EmployerIncentives;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesService : IEmployerIncentivesService
    {
        private readonly IRestApiClient _client;

        public EmployerIncentivesService(HttpClient httpClient)
        {
            _client = new RestApiClient(httpClient);
        }

        public async Task<bool> IsHealthy(CancellationToken cancellationToken = default)
        {
            try
            {
                var status = await _client.GetHttpStatusCode("/ping", null, cancellationToken);
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship, CancellationToken cancellationToken = default)
        {
            ConcurrentBag<ApprenticeshipItem> bag = new ConcurrentBag<ApprenticeshipItem>();
            var tasks = allApprenticeship.Select(x => VerifyApprenticeshipIsEligible(x, bag, cancellationToken));
            await Task.WhenAll(tasks);

            return bag.ToArray();
        }

        public Task CreateIncentiveApplication(CreateIncentiveApplicationRequest request, CancellationToken cancellationToken = default)
        {
            return _client.Post("/applications", request, cancellationToken);
        }

        private async Task VerifyApprenticeshipIsEligible(ApprenticeshipItem apprenticeship, ConcurrentBag<ApprenticeshipItem> bag, CancellationToken cancellationToken)
        {
            var statusCode = await _client.GetHttpStatusCode($"eligible-apprenticeships/{apprenticeship.Uln}", new { StartDate = apprenticeship.StartDate.ToString("yyyy-MM-dd"), IsApproved = true }, cancellationToken);
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
