using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostAddDraftApprenticeshipsRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public object Data { get; set; }
        public string PostUrl => $"api/{ProviderId}/bulkupload";

        public PostAddDraftApprenticeshipsRequest(long providerId, BulkUploadAddDraftApprenticeshipsRequest data)
        {
            ProviderId = providerId;
            Data = data;
        }
    }
}
