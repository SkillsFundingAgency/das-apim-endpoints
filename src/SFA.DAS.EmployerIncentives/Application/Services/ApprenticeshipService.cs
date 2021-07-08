using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class ApprenticeshipService : IApprenticeshipService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public ApprenticeshipService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship)
        {
            var bag = new ConcurrentBag<ApprenticeshipItem>();
            var tasks = allApprenticeship.Select(x => VerifyApprenticeshipIsEligible(x, bag));
            await Task.WhenAll(tasks);

            return bag.ToArray();
        }
        
        private async Task VerifyApprenticeshipIsEligible(ApprenticeshipItem apprenticeship, ConcurrentBag<ApprenticeshipItem> bag)
        {
            var statusCode = await _client.GetResponseCode(new GetEligibleApprenticeshipsRequest(apprenticeship.Uln, apprenticeship.StartDate));
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
