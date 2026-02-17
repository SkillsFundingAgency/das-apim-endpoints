using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveReviewerApiRequest : IPutApiRequest
{
    public Guid ApplicationId { get; }
    public SaveReviewerApiRequest(Guid applicationId)
    {
        ApplicationId = applicationId;
    }

    public string PutUrl => $"/api/applications/{ApplicationId}/reviewer";

    public object Data { get; set; }
}
