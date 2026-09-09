using SFA.DAS.Apim.Shared.Interfaces;

public class GetApplicationExportDetailsApiRequest : IGetApiRequest
{
    public Guid ApplicationReviewId { get; set; }

    public GetApplicationExportDetailsApiRequest(Guid applicationReviewId)
    {
        ApplicationReviewId = applicationReviewId;
    }

    public string GetUrl => $"/api/application-reviews/{ApplicationReviewId}/export-data";

}
