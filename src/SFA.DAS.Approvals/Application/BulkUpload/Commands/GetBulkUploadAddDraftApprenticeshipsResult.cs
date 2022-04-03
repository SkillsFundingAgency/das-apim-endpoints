using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class GetBulkUploadAddDraftApprenticeshipsResult
    {
        public GetBulkUploadAddDraftApprenticeshipsResult()
        {
            BulkUploadAddDraftApprenticeshipsResponse = new List<BulkUploadAddDraftApprenticeshipsResult>();
        }

        public IEnumerable<BulkUploadAddDraftApprenticeshipsResult> BulkUploadAddDraftApprenticeshipsResponse { get; set; }
    }
}
