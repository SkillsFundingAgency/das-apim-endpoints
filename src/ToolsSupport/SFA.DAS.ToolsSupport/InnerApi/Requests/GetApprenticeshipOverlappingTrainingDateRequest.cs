using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprenticeshipOverlappingTrainingDateRequest(long id) : IGetApiRequest
{
    public readonly long Id = id;
    public string GetUrl => $"api/overlapping-training-date-request/{Id}";
}
