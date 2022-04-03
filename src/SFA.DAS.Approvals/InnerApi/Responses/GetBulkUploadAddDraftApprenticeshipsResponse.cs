using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetBulkUploadAddDraftApprenticeshipsResponse
    {
        public GetBulkUploadAddDraftApprenticeshipsResponse()
        {
            BulkUploadAddDraftApprenticeshipsResponse = new List<BulkUploadAddDraftApprenticeshipsResponse>();
        }

        public IEnumerable<BulkUploadAddDraftApprenticeshipsResponse> BulkUploadAddDraftApprenticeshipsResponse { get; set; }
    }
}
