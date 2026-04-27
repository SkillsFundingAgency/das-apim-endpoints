using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange
{
	public class GetPendingPriceChangeRequest : IGetApiRequest
	{
		public readonly Guid LearningKey;

		public GetPendingPriceChangeRequest(Guid learningKey)
		{
			LearningKey = learningKey;
		}

		public string GetUrl => $"{LearningKey}/priceHistory/pending";
	}
}
