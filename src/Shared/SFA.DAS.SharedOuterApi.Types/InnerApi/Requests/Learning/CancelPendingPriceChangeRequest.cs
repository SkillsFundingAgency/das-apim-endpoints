using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning
{
    public class CancelPendingPriceChangeRequest : IDeleteApiRequest
    {
	    public CancelPendingPriceChangeRequest(Guid apprenticeshipKey)
	    {
		    ApprenticeshipKey = apprenticeshipKey;
	    }

	    public Guid ApprenticeshipKey { get; }
        public string DeleteUrl => $"{ApprenticeshipKey}/priceHistory/pending";
    }
}
