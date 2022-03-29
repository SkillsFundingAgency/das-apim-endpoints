using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries
{
    public class GetCohortQueryHandler : IRequestHandler<GetCohortQuery, GetCohortResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetCohortQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetCohortResult> Handle(GetCohortQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetCohortResponse>(new GetCohortRequest(request.CohortId));

            if (result == null)
                return null;

            return new GetCohortResult
            {
                IsLinkedToChangeOfPartyRequest = result.IsLinkedToChangeOfPartyRequest,
                TransferApprovalStatus = (Shared.Enums.TransferApprovalStatus?) result.TransferApprovalStatus,
                ChangeOfPartyRequestId = result.ChangeOfPartyRequestId,
                LevyStatus = (Shared.Enums.ApprenticeshipEmployerType) result.LevyStatus,
                IsCompleteForProvider = result.IsCompleteForProvider,
                IsCompleteForEmployer = result.IsCompleteForEmployer,
                IsApprovedByProvider = result.IsApprovedByProvider,
                IsApprovedByEmployer = result.IsApprovedByEmployer,
                LastAction = (Shared.Enums.LastAction) result.LastAction,
                LatestMessageCreatedByProvider = result.LatestMessageCreatedByProvider,
                WithParty = (Shared.Enums.Party) result.WithParty,
                PledgeApplicationId = result.PledgeApplicationId,
                TransferSenderId = result.TransferSenderId,
                IsFundedByTransfer = result.IsFundedByTransfer,
                ProviderName = result.ProviderName,
                LegalEntityName = result.LegalEntityName,
                AccountLegalEntityId = result.AccountLegalEntityId,
                CohortId = result.CohortId,
                LatestMessageCreatedByEmployer = result.LatestMessageCreatedByEmployer,
                ApprenticeEmailIsRequired = result.ApprenticeEmailIsRequired
            };
        }
    }
}
