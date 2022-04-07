using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetBulkUploadAddDraftApprenticeshipsResponse
    {
        public IEnumerable<BulkUploadAddDraftApprenticeshipsResponse> BulkUploadAddDraftApprenticeshipsResponse { get; set; } = new List<BulkUploadAddDraftApprenticeshipsResponse>();
    }
}
