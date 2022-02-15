using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Reference;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
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

        public async Task<ApiResponse<ClosePledgeRequest>> ClosePledge(ClosePledgeRequest request)
        {
            return await _levyTransferMatchingApiClient.PostWithResponseCode<ClosePledgeRequest>(request);
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

        public async Task<GetAccountsResponse> GetAccounts(GetAccountsRequest request)
        {
            return await _levyTransferMatchingApiClient.Get<GetAccountsResponse>(request);
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

        public async Task<ApiResponse<DebitPledgeRequest>> DebitPledge(DebitPledgeRequest request)
        {
            return await _levyTransferMatchingApiClient.PostWithResponseCode<DebitPledgeRequest>(request);
        }

        public async Task<ApiResponse<UndoApplicationApprovalRequest>> UndoApplicationApproval(UndoApplicationApprovalRequest request)
        {
            return await _levyTransferMatchingApiClient.PostWithResponseCode<UndoApplicationApprovalRequest>(request);
        }

        public async Task ApproveApplication(ApproveApplicationRequest request)
        {
            await _levyTransferMatchingApiClient.PostWithResponseCode<ApproveApplicationRequest>(request);
        }

        public async Task RejectApplication(RejectApplicationRequest request)
        {
            await _levyTransferMatchingApiClient.PostWithResponseCode<RejectApplicationRequest>(request);
        }

        public async Task<ApiResponse<AcceptFundingRequest>> AcceptFunding(AcceptFundingRequest request, CancellationToken cancellationToken = default)
        {
            return await _levyTransferMatchingApiClient.PostWithResponseCode<AcceptFundingRequest>(request);
        }

        public async Task WithdrawApplication(WithdrawApplicationRequest request, CancellationToken cancellationToken = default)
        {
            await _levyTransferMatchingApiClient.PostWithResponseCode<WithdrawApplicationRequest>(request);
        }

        public async Task<ApiResponse<DebitApplicationRequest>> DebitApplication(DebitApplicationRequest request)
        {
            return await _levyTransferMatchingApiClient.PostWithResponseCode<DebitApplicationRequest>(request);
        }

        public async Task<ApiResponse<DeclineFundingRequest>> DeclineFunding(DeclineFundingRequest request)
        {
            return await _levyTransferMatchingApiClient.PostWithResponseCode<DeclineFundingRequest>(request);
        }

        public async Task<ApiResponse<GenerateMatchingCriteriaRequest>> GenerateMatchingCriteria(GenerateMatchingCriteriaRequest request)
        {
            return await _levyTransferMatchingApiClient.PostWithResponseCode<GenerateMatchingCriteriaRequest>(request);
        }

        public async Task<ApiResponse<CreditPledgeRequest>> CreditPledge(CreditPledgeRequest request)
        {
            return await _levyTransferMatchingApiClient.PostWithResponseCode<CreditPledgeRequest>(request);
        }
    }
}