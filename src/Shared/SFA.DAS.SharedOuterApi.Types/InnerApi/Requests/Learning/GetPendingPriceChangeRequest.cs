using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning
{
    public class GetPendingPriceChangeRequest : IGetApiRequest
    {
	    public GetPendingPriceChangeRequest(Guid apprenticeshipKey)
	    {
		    ApprenticeshipKey = apprenticeshipKey;
	    }

	    public Guid ApprenticeshipKey { get; }
        public string GetUrl => $"{ApprenticeshipKey}/priceHistory/pending";
    }
}
