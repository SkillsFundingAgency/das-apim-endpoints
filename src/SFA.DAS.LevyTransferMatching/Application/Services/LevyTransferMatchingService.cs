using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Tags;
using SFA.DAS.LevyTransferMatching.Models.Tags;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Application.Services
{
    public class LevyTransferMatchingService : ILevyTransferMatchingService
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public LevyTransferMatchingService(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<IEnumerable<Tag>> GetLevels()
        {
            return await _levyTransferMatchingApiClient.GetAll<Tag>(new GetLevelsRequest());
        }

        public async Task<IEnumerable<Tag>> GetSectors()
        {
            return await _levyTransferMatchingApiClient.GetAll<Tag>(new GetSectorsRequest());
        }

        public async Task<IEnumerable<Tag>> GetJobRoles()
        {
            return await _levyTransferMatchingApiClient.GetAll<Tag>(new GetJobRolesRequest());
        }

        public async Task<int> CreatePledge(Pledge pledge)
        {
            var apiResponse = await _levyTransferMatchingApiClient.PostWithResponseCode<PledgeReference>(
                new CreatePledgeRequest(pledge.AccountId)
                {
                    Data = pledge,
                });

            return apiResponse.Body.Id.Value;
        }

        public async Task<IEnumerable<Pledge>> GetPledges()
        {
            var response = await _levyTransferMatchingApiClient.GetAll<Pledge>(new GetPledgesRequest());

            return response;
        }
    }
}