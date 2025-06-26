using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Funding.InnerApi.Requests.Learning;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.Application.Services
{
    public class ApprenticeshipsService : IApprenticeshipsService
    {
        private readonly ILearningApiClient<LearningApiConfiguration> _client;

        public ApprenticeshipsService(ILearningApiClient<LearningApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<ApprenticeshipsDto> GetAll(long ukprn)
        {
            var response = await _client.GetAll<LearningDto>(new GetLearningsRequest(ukprn));

            return new ApprenticeshipsDto() { Apprenticeships = response.ToList()};
        }
    }
}