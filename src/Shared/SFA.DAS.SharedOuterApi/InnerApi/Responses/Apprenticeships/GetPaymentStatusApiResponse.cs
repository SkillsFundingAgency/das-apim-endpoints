using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;

public class GetPaymentStatusApiResponse
{
	public bool PaymentsFrozen { get; set; }
    public string ReasonFrozen { get; set; }
    public DateTime? FrozenOn { get; set; }
}