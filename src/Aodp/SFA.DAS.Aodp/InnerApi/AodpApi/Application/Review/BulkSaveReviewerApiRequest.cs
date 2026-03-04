using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.Aodp.InnerApi.Application.Review;
{
    public class BulkSaveReviewerApiRequest : IPutApiRequest
{
    public string PutUrl => $"/api/applications/bulk-reviewer";
    public object Data { get; set; }
}
