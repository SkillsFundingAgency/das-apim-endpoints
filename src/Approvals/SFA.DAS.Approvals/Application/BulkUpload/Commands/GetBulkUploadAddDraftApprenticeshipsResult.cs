using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class GetBulkUploadAddDraftApprenticeshipsResult
    {
        public IEnumerable<BulkUploadAddDraftApprenticeshipsResult> BulkUploadAddDraftApprenticeshipsResponse { get; set; } = new List<BulkUploadAddDraftApprenticeshipsResult>();
    }
}
