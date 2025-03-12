using SFA.DAS.SharedOuterApi.Interfaces;

public class SubmitApplicationApiRequest : IPutApiRequest
{
    public Guid ApplicationId { get; set; }

    public string PutUrl => $"/api/applications/{ApplicationId}/submit";

    public object Data { get; set; }

}