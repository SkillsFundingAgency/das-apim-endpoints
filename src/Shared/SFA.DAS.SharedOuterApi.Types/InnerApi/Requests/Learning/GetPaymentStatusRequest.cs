using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning;

public class GetPaymentStatusRequest : IGetApiRequest
{
	public GetPaymentStatusRequest(Guid learningKey)
	{
		LearningKey = learningKey;
	}

	public Guid LearningKey { get; }
	public string GetUrl => $"{LearningKey}/paymentStatus";
}