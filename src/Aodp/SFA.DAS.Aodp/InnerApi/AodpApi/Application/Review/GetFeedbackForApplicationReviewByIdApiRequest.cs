using SFA.DAS.SharedOuterApi.Interfaces;

public class GetFeedbackForApplicationReviewByIdApiRequest : IGetApiRequest
{
    public Guid ApplicationReviewId { get; set; }

    public string UserType { get; set; }

    public GetFeedbackForApplicationReviewByIdApiRequest(Guid applicationReviewId, string userType)
    {
        ApplicationReviewId = applicationReviewId;
        UserType = userType;
    }

    public string GetUrl => $"/api/application-reviews/{ApplicationReviewId}/feedback/{UserType}";

}