using SFA.DAS.Approvals.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkUploadAddDraftApprenticeshipsRequest : SaveDataRequest
    {
        public long ProviderId { get; set; }
        public long? FileUploadLogId { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> BulkUploadDraftApprenticeships { get; set; }
    }
}
