using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetPriceEpisodesResponse;
using System.Collections.Generic;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetChangeOfPartyRequestsResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetChangeOfProviderChainResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetOverlappingTrainingDateResponse;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails
{
    public class GetManageApprenticeshipDetailsQueryResult
    {
        public GetApprenticeshipResponse Apprenticeship{ get; set; }
        public IReadOnlyCollection<PriceEpisode> PriceEpisodes { get; set; }
        public IReadOnlyCollection<ApprenticeshipUpdate> ApprenticeshipUpdates { get; set; }
        public IReadOnlyCollection<GetDataLocksResponse.DataLock> DataLocks { get; set; }
        public IReadOnlyCollection<ChangeOfPartyRequest> ChangeOfPartyRequests { get; set; }
        public IReadOnlyCollection<ChangeOfProviderLink> ChangeOfProviderChain { get; set; }
        public IReadOnlyCollection<ApprenticeshipOverlappingTrainingDateRequest> OverlappingTrainingDateRequest { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
    }
}