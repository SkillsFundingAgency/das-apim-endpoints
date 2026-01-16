using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveReviewerApiRequest(Guid applicationId) : IPutApiRequest
{
    public string PutUrl => $"/api/applications/{applicationId}/reviewer";

    public object Data { get; set; }
}
