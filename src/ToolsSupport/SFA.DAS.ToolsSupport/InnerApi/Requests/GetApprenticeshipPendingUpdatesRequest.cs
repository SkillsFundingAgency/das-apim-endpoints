using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprenticeshipPendingUpdatesRequest(long id) : IGetApiRequest
{
    public readonly long Id = id;
    public string GetUrl => $"api/apprenticeships/{Id}/updates";
}
