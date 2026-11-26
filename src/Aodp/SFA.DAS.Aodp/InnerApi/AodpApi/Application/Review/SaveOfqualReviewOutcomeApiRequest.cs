using SFA.DAS.Apim.Shared.Interfaces;

public class SaveOfqualReviewOutcomeApiRequest(Guid applicationReviewId) : IPutApiRequest
{
    public string PutUrl => $"/api/application-reviews/{applicationReviewId}/ofqual-outcome";

    public object Data { get; set; }
}
