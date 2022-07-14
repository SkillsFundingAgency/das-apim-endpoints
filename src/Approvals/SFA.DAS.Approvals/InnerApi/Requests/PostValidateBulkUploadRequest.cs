using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostValidateBulkUploadRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public object Data { get; set; }
        public string PostUrl => $"api/{ProviderId}/bulkupload/validate";

        public PostValidateBulkUploadRequest(long providerId, BulkUploadValidateApiRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }
    }
}
