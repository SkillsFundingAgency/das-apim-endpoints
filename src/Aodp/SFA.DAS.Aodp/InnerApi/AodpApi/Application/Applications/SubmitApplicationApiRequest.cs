using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

public class SubmitApplicationApiRequest : IPutApiRequest
{
    public Guid ApplicationId { get; set; }

    public string PutUrl => $"/api/applications/{ApplicationId}/submit";

    public object Data { get; set; }

}