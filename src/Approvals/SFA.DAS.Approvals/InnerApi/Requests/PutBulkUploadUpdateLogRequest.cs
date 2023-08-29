using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PutBulkUploadUpdateLogRequest : IPutApiRequest
    {
        public long ProviderId { get; }
        public long LogId { get; }
        public object Data { get; set; }

        public string PutUrl => $"api/{ProviderId}/bulkupload/update-log";

        public PutBulkUploadUpdateLogRequest(long logId, long providerId, BulkUploadUpdateLogRequest data)
        {
            ProviderId = providerId;
            LogId = logId;
            Data = data;
        }
    }
}
