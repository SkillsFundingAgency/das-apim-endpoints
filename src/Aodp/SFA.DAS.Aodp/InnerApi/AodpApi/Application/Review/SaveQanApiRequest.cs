using SFA.DAS.Apim.Shared.Interfaces;

public class SaveQanApiRequest(Guid applicationReviewId) : IPutApiRequest
{
    public string PutUrl => $"/api/application-reviews/{applicationReviewId}/qan";

    public object Data { get; set; }
}
