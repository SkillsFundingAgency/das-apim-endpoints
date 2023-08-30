using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostBulkUploadUpdateLogRequest : IPostApiRequest
    {
        public long ProviderId { get; }
        public object Data { get; set; }
        public string PostUrl => $"api/{ProviderId}/bulkupload/update-log";

        public PostBulkUploadUpdateLogRequest(long providerId, BulkUploadUpdateLogRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }
    }
}