namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Learning;

public class GetPaymentStatusApiResponse
{
	public bool PaymentsFrozen { get; set; }
    public string ReasonFrozen { get; set; }
    public DateTime? FrozenOn { get; set; }
}