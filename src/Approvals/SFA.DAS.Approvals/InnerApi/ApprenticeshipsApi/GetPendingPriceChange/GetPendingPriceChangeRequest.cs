using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange
{
	public class GetPendingPriceChangeRequest : IGetApiRequest
	{
		public readonly Guid ApprenticeshipKey;

		public GetPendingPriceChangeRequest(Guid apprenticeshipKey)
		{
			ApprenticeshipKey = apprenticeshipKey;
		}

		public string GetUrl => $"{ApprenticeshipKey}/priceHistory/pending";
	}
}
