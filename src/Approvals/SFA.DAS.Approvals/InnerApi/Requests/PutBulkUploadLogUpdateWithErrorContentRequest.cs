using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PutBulkUploadLogUpdateWithErrorContentRequest(
        long providerId,
        long logId,
        BulkUploadLogUpdateWithErrorContentRequest data)
        : IPutApiRequest
    {
        public long ProviderId { get; } = providerId;
        public long LogId { get; } = logId;
        public object Data { get; set; } = data;
        public string PutUrl => $"api/{ProviderId}/bulkupload/logs/{LogId}/error";
    }
}