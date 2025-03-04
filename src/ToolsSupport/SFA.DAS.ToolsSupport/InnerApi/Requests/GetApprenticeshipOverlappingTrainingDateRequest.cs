using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Requests;

public class GetApprenticeshipOverlappingTrainingDateRequest : IGetApiRequest
{
    public readonly long Id;
    public string GetUrl => $"api/overlapping-training-date-request/{Id}";

    public GetApprenticeshipOverlappingTrainingDateRequest(long id)
    {
        Id = id;
    }
}
