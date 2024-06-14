using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class GetPaymentStatusRequest : IGetApiRequest
{
	public GetPaymentStatusRequest(Guid apprenticeshipKey)
	{
		ApprenticeshipKey = apprenticeshipKey;
	}

	public Guid ApprenticeshipKey { get; }
	public string GetUrl => $"{ApprenticeshipKey}/paymentStatus";
}