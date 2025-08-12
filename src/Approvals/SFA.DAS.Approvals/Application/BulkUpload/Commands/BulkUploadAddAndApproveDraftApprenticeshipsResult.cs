using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsResult
    {
        public IEnumerable<BulkUploadAddDraftApprenticeshipsResult> BulkUploadAddAndApproveDraftApprenticeshipResponse { get; set; } = new List<BulkUploadAddDraftApprenticeshipsResult>();
    }
}
