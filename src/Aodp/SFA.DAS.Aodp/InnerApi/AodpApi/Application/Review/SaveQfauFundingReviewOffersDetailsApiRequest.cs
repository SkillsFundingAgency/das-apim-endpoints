using SFA.DAS.Apim.Shared.Interfaces;

public class SaveQfauFundingReviewOffersDetailsApiRequest : IPutApiRequest
{
    public Guid ApplicationReviewId { get; set; }

    public string PutUrl => $"/api/application-reviews/{ApplicationReviewId}/qfau-offer-details";
    public object Data { get; set; }
}
