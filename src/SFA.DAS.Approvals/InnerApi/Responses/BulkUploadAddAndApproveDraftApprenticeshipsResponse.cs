using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsResponse
    {
        public BulkUploadAddAndApproveDraftApprenticeshipsResponse() { BulkUploadAddAndApproveDraftApprenticeshipResponse = new List<BulkUploadAddDraftApprenticeshipsResponse>(); }

        public IEnumerable<BulkUploadAddDraftApprenticeshipsResponse> BulkUploadAddAndApproveDraftApprenticeshipResponse { get; set; }
    }
}
