using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PutBulkUploadLogUpdateWithErrorContentRequest : IPutApiRequest
    {
        public long ProviderId { get; }
        public long LogId { get; }
        public object Data { get; set; }
        public string PutUrl => $"api/{ProviderId}/bulkupload/logs/{LogId}/error";

        public PutBulkUploadLogUpdateWithErrorContentRequest(long providerId, long logId, BulkUploadLogUpdateWithErrorContentRequest data)
        {
            ProviderId = providerId;
            LogId = logId;
            Data = data;
        }
    }
}