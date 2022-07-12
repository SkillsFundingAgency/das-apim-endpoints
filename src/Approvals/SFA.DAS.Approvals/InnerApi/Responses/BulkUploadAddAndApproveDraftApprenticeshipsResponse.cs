using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsResponse
    {
        public IEnumerable<BulkUploadAddDraftApprenticeshipsResponse> BulkUploadAddAndApproveDraftApprenticeshipResponse { get; set; } = new List<BulkUploadAddDraftApprenticeshipsResponse>();

    }
}
