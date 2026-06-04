using System;
using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetChangeOfEmployerChainResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetChangeOfPartyRequestsResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetChangeOfProviderChainResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetOverlappingTrainingDateResponse;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetPriceEpisodesResponse;

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
        public IReadOnlyCollection<ChangeOfEmployerLink> ChangeOfEmployerChain { get; set; }
        public IReadOnlyCollection<ApprenticeshipOverlappingTrainingDateRequest> OverlappingTrainingDateRequest { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public bool? CanActualStartDateBeChanged { get; set; }
    }
}