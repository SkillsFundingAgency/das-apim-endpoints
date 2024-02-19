using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
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
