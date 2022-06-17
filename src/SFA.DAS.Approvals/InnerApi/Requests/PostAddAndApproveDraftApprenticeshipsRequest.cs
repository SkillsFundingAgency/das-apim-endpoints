using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddAndApproveDraftApprenticeshipsRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public object Data { get; set; }

        public string PostUrl => $"api/{ProviderId}/bulkupload/AddAndApprove";

        public PostAddAndApproveDraftApprenticeshipsRequest(long providerId, BulkUploadAddAndApproveDraftApprenticeshipsRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }
    }
}
