using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveQfauFundingReviewOffersDetailsApiRequest : IPutApiRequest
{
    public Guid ApplicationReviewId { get; set; }

    public string PutUrl => $"/api/application-reviews/{ApplicationReviewId}/save-qfau-offer-details";
    public object Data { get; set; }
}
