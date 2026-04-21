using SFA.DAS.Apim.Shared.Interfaces;

public class SaveReviewOwnerUpdateApiRequest(Guid applicationReviewId) : IPutApiRequest
{
    public string PutUrl => $"/api/application-reviews/{applicationReviewId}/owner";

    public object Data { get; set; }
}
