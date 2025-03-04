using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprenticeshipPendingUpdatesRequest : IGetApiRequest
{
    public readonly long Id;
    public string GetUrl => $"api/apprenticeships/{Id}/updates";

    public GetApprenticeshipPendingUpdatesRequest(long id)
    {
        Id = id;
    }
}
