using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprenticeshipPriceEpisodeRequest : IGetApiRequest
{
    public readonly long Id;
    public string GetUrl => $"api/apprenticeships/{Id}/price-episodes";

    public GetApprenticeshipPriceEpisodeRequest(long id)
    {
        Id = id;
    }
}
