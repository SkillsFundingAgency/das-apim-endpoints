using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange
{
    public class GetPendingPriceChangeRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;

        public GetPendingPriceChangeRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"{ApprenticeshipId}/priceHistory/requested";
    }
}
