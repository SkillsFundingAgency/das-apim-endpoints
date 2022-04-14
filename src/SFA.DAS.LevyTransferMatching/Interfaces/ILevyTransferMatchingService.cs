using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using GetAccountRequest = SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.GetAccountRequest;
using Pledge = SFA.DAS.LevyTransferMatching.Models.Pledge;

namespace SFA.DAS.LevyTransferMatching.Interfaces
{
    public interface ILevyTransferMatchingService
    {
        Task<IEnumerable<ReferenceDataItem>> GetLevels();
        Task<IEnumerable<ReferenceDataItem>> GetSectors();
        Task<IEnumerable<ReferenceDataItem>> GetJobRoles();
		Task<GetPledgesResponse> GetPledges(GetPledgesRequest request);
        Task<CreatePledgeResponse> CreatePledge(CreatePledgeRequest pledge);
        Task<ApiResponse<ClosePledgeRequest>> ClosePledge(ClosePledgeRequest request);
        Task<GetAccountResponse> GetAccount(GetAccountRequest request);
        Task<GetAccountsResponse> GetAccounts(GetAccountsRequest request);
        Task CreateAccount(CreateAccountRequest request);
        Task<Pledge> GetPledge(int id);
        Task<CreateApplicationResponse> CreateApplication(CreateApplicationRequest request);
        Task<GetApplicationsResponse> GetApplications(GetApplicationsRequest request);
        Task<ApiResponse<CreditPledgeRequest>> CreditPledge(CreditPledgeRequest request);
        Task<GetApplicationResponse> GetApplication(GetApplicationRequest request);

        Task<ApiResponse<DebitPledgeRequest>> DebitPledge(DebitPledgeRequest request);
        Task<ApiResponse<UndoApplicationApprovalRequest>> UndoApplicationApproval(UndoApplicationApprovalRequest request);
        Task ApproveApplication(ApproveApplicationRequest request);
        Task RejectApplication(RejectApplicationRequest request);
        Task<ApiResponse<AcceptFundingRequest>> AcceptFunding(AcceptFundingRequest request, CancellationToken cancellationToken = default);
        Task WithdrawApplication(WithdrawApplicationRequest request, CancellationToken cancellationToken = default);
        Task<ApiResponse<DebitApplicationRequest>> DebitApplication(DebitApplicationRequest request);
        Task<ApiResponse<DeclineFundingRequest>> DeclineFunding(DeclineFundingRequest request);
        Task<ApiResponse<GenerateMatchingCriteriaRequest>> GenerateMatchingCriteria(GenerateMatchingCriteriaRequest request);
    }
}