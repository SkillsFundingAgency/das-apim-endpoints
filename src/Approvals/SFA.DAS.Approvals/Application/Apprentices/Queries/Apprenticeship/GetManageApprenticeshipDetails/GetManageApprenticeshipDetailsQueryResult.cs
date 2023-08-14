using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetPriceEpisodesResponse;
using System.Collections.Generic;
using static SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails
{
    public class GetManageApprenticeshipDetailsQueryResult
    {
        public GetApprenticeshipResponse Apprenticeship{ get; set; }
        public IReadOnlyCollection<PriceEpisode> PriceEpisodes { get; set; }
        public IReadOnlyCollection<ApprenticeshipUpdate> ApprenticeshipUpdates { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
    }
}