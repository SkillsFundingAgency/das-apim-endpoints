using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsResult
    {
        public BulkUploadAddAndApproveDraftApprenticeshipsResult() { BulkUploadAddAndApproveDraftApprenticeshipResponse = new List<BulkUploadAddDraftApprenticeshipsResult>(); }

        public IEnumerable<BulkUploadAddDraftApprenticeshipsResult> BulkUploadAddAndApproveDraftApprenticeshipResponse { get; set; }
    }
}
