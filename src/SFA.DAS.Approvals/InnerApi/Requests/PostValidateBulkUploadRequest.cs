using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostValidateBulkUploadRequest : IPostApiRequest
    {
        public string PostUrl => $"api/{ProviderId}/bulkupload/validate";

        public PostValidateBulkUploadRequest(long providerId, BulkUploadAddDraftApprenticeshipsRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }

        public long ProviderId { get; }
        public object Data { get ; set ; }
    }
}
