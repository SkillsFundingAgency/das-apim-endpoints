using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostBulkUploadAddLogRequest : IPostApiRequest
    {
        public long ProviderId { get; }
        public object Data { get; set; }
        public string PostUrl => $"api/{ProviderId}/bulkupload/add-log";

        public PostBulkUploadAddLogRequest(long providerId, BulkUploadAddLogRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }
    }
}