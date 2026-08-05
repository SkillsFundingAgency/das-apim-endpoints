using SFA.DAS.Apim.Shared.Interfaces;

public class GetApplicationsForReviewApiRequest : IPostApiRequest
{
    public string PostUrl => $"/api/application-reviews";
    public object Data { get; set; }
}
