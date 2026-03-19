namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Learning
{
    public class GetPendingPriceChangeApiResponse
	{
		public bool HasPendingPriceChange { get; set; }
		public PendingPriceChange PendingPriceChange { get; set; }
	}
}