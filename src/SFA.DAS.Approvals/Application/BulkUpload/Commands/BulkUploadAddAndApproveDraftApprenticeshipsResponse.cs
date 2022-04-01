using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsResponse
    {
        public BulkUploadAddAndApproveDraftApprenticeshipsResponse() { BulkUploadAddAndApproveDraftApprenticeshipResponse = new List<BulkUploadAddDraftApprenticeshipsResponse>(); }

        public IEnumerable<BulkUploadAddDraftApprenticeshipsResponse> BulkUploadAddAndApproveDraftApprenticeshipResponse { get; set; }
    }
}
