namespace SFA.DAS.Apprenticeships.Api.Models;

public class PostCreateApprenticeshipPriceChangeResponse
{
	public PostCreateApprenticeshipPriceChangeResponse(string priceChangeStatus)
	{
		PriceChangeStatus = priceChangeStatus;
	}

	public string PriceChangeStatus { get; set; }
}