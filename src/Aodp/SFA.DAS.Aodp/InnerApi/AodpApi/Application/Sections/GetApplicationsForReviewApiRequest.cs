using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationsForReviewApiRequest : IPostApiRequest
{
    public string PostUrl => $"/api/application-reviews";
    public object Data { get; set; }
}
