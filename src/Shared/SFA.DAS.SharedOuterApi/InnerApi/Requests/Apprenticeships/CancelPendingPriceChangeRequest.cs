using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
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
