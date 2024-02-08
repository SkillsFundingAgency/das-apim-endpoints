using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Funding.InnerApi.Requests.Apprenticeships;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.Application.Services
{
    public class ApprenticeshipsService : IApprenticeshipsService
    {
        private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _client;

        public ApprenticeshipsService(IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<ApprenticeshipsDto> GetAll(long ukprn)
        {
            var response = await _client.GetAll<ApprenticeshipDto>(new GetApprenticeshipsRequest(ukprn));

            return new ApprenticeshipsDto() { Apprenticeships = response.ToList()};
        }
    }
}