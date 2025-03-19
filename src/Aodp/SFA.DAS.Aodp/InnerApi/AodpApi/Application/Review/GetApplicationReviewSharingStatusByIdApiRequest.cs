using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationReviewSharingStatusByIdApiRequest : IGetApiRequest
{
    public Guid ApplicationReviewId { get; set; }

    public GetApplicationReviewSharingStatusByIdApiRequest(Guid applicationReviewId)
    {
        ApplicationReviewId = applicationReviewId;
    }

    public string GetUrl => $"/api/application-reviews/{ApplicationReviewId}/share-status";

}
