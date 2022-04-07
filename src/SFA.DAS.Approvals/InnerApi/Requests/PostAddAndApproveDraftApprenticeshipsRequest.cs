using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddAndApproveDraftApprenticeshipsRequest : IPostApiRequest
    {
        private readonly long _providerId;
        public object Data { get; set; }

        public string PostUrl => $"api/{_providerId}/bulkupload/AddAndApprove";

        public PostAddAndApproveDraftApprenticeshipsRequest(long providerId, BulkUploadAddAndApproveDraftApprenticeshipsRequest data)
        {
            _providerId = providerId;
            Data = data;
        }
    }
}
