using SFA.DAS.Apim.Shared.Interfaces;

public class UpdateApplicationReviewSharingApiRequest(Guid applicationReviewId) : IPutApiRequest
{
    public string PutUrl => $"/api/application-reviews/{applicationReviewId}/share";

    public object Data { get; set; }
}
