using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostValidateBulkUploadRequest : IPostApiRequest
    {
        private readonly long _providerId;
        public object Data { get; set; }
        public string PostUrl => $"api/{_providerId}/bulkupload/validate";

        public PostValidateBulkUploadRequest(long providerId, BulkUploadValidateApiRequest data)
        {
            _providerId = providerId;
            Data = data;
        }
    }
}
