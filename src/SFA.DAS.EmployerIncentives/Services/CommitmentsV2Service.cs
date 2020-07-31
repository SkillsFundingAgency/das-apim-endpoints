using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<bool> IsHealthy(CancellationToken cancellationToken = default)
        {
            try
            {
                var status = await _restApiClient.GetHttpStatusCode("api/ping", cancellationToken);
                    
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
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
