using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Funding.Configuration;
using SFA.DAS.Funding.InnerApi.Requests.Apprenticeships;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;

namespace SFA.DAS.Funding.Application.Services
{
    public class ApprenticeshipsService : IApprenticeshipsService
    {
        private readonly IApprenticeshipsApiClient<ApprenticeshipsConfiguration> _client;

        public ApprenticeshipsService(IApprenticeshipsApiClient<ApprenticeshipsConfiguration> client)
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