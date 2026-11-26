using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostBulkUploadAddLogRequest : IPostApiRequest
    {
        public long ProviderId { get; }
        public object Data { get; set; }
        public string PostUrl => $"api/{ProviderId}/bulkupload/logs";

        public PostBulkUploadAddLogRequest(long providerId, BulkUploadAddLogRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }
    }
}