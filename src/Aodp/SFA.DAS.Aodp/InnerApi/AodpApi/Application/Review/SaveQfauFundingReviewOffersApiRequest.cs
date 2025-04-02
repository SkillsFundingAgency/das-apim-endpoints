using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveQfauFundingReviewOffersApiRequest : IPutApiRequest
{
    public Guid ApplicationReviewId { get; set; }
    public string PutUrl => $"/api/application-reviews/{ApplicationReviewId}/qfau-offers";
    public object Data { get; set; }
}
