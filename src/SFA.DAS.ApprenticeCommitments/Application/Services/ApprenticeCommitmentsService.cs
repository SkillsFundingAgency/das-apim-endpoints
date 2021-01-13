using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.ApprenticeCommitments.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class ApprenticeCommitmentsService //: IEmployerIncentivesService
    {
        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _client;

        public ApprenticeCommitmentsService(IInternalApiClient<ApprenticeCommitmentsConfiguration> client)
            => _client = client;

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _client.GetResponseCode(new GetHealthRequest());
                return status == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public async Task CreateApprenticeship(Guid guid, long apprenticeshipId, string email)
        {
            var response = await _client.Post<CreateApprenticeshipResponse>(new CreateApprenticeshipRequest
            {
                Data = new CreateApprenticeshipRequestData
                {
                    RequestId = guid,
                    ApprenticeshipId = apprenticeshipId,
                    Email = email,
                }
            });
        }
    }
}