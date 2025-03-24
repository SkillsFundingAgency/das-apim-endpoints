using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveQfauFundingReviewOutcomeApiRequest : IPutApiRequest
{
    public Guid ApplicationReviewId { get; set; }
    public string PutUrl => $"/api/application-reviews/{ApplicationReviewId}/save-qfau-outcome";
    public object Data { get; set; }
}
