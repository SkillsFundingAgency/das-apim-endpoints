using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddAndApproveDraftApprenticeshipsRequest : IPostApiRequest
    {
        public string PostUrl => $"api/{ProviderId}/bulkupload/AddAndApprove";

        public PostAddAndApproveDraftApprenticeshipsRequest(long providerId, BulkUploadAddAndApproveDraftApprenticeshipsRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }

        public long ProviderId { get; }
        public object Data { get; set; }
    }
}
