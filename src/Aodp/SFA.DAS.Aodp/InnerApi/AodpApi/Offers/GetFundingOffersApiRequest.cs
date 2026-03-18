using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

public class GetFundingOffersApiRequest : IGetApiRequest
{
    public Guid ApplicationId { get; set; }

    public string GetUrl => $"/api/funding-offers";

}