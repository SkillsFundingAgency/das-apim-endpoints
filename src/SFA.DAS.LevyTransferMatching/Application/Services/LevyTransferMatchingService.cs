using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using Pledge = SFA.DAS.LevyTransferMatching.Models.Pledge;

namespace SFA.DAS.LevyTransferMatching.Application.Services
{
    public class LevyTransferMatchingService : ILevyTransferMatchingService
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public LevyTransferMatchingService(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<IEnumerable<ReferenceDataItem>> GetLevels()
        {
            return await _levyTransferMatchingApiClient.GetAll<ReferenceDataItem>(new GetLevelsRequest());
        }

        public async Task<IEnumerable<ReferenceDataItem>> GetSectors()
        {
            return await _levyTransferMatchingApiClient.GetAll<ReferenceDataItem>(new GetSectorsRequest());
        }

        public async Task<IEnumerable<ReferenceDataItem>> GetJobRoles()
        {
            return await _levyTransferMatchingApiClient.GetAll<ReferenceDataItem>(new GetJobRolesRequest());
        }

        public async Task<CreatePledgeResponse> CreatePledge(CreatePledgeRequest request)
        {
            var response = await _levyTransferMatchingApiClient.PostWithResponseCode<CreatePledgeResponse>(request);
            return response.Body;
        }

        public async Task<GetPledgesResponse> GetPledges(GetPledgesRequest request)
        {
            var response = await _levyTransferMatchingApiClient.Get<GetPledgesResponse>(request);

            return response;
        }

        public async Task<GetAccountResponse> GetAccount(GetAccountRequest request)
        {
            return await _levyTransferMatchingApiClient.Get<GetAccountResponse>(request);
        }

        public async Task CreateAccount(CreateAccountRequest request)
        {
            await _levyTransferMatchingApiClient.PostWithResponseCode<CreateAccountRequest>(request);
        }

        public async Task<Pledge> GetPledge(int id)
        {
            var response = await _levyTransferMatchingApiClient.Get<Pledge>(new GetPledgeRequest(id));

            return response;
        }

        public async Task<CreateApplicationResponse> CreateApplication(CreateApplicationRequest request)
        {
           var response = await _levyTransferMatchingApiClient.PostWithResponseCode<CreateApplicationResponse>(request);
           return response.Body;
        }

        public async Task<GetApplicationResponse> GetApplication(GetApplicationRequest request)
        {
            var response = await _levyTransferMatchingApiClient.Get<GetApplicationResponse>(request);

            return response;
        }

        public async Task<GetApplicationsResponse> GetApplications(GetApplicationsRequest request)
        {
            return await _levyTransferMatchingApiClient.Get<GetApplicationsResponse>(request);
        }
    }
}