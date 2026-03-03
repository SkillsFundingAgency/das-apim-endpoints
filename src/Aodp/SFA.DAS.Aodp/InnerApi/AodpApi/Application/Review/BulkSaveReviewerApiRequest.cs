using SFA.DAS.SharedOuterApi.Interfaces;

public class BulkSaveReviewerApiRequest : IPutApiRequest
{
    public string PutUrl => $"/api/applications/bulk-reviewer";
    public object Data { get; set; }
}
