using SFA.DAS.SharedOuterApi.Interfaces;

public class GetFundingOffers : IGetApiRequest
{
    public Guid ApplicationId { get; set; }

    public string GetUrl => $"/api/funding-offers";

}