using SFA.DAS.Approvals.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class BulkUploadAddDraftApprenticeshipsRequest
    {
        public BulkUploadAddDraftApprenticeshipsRequest()
        {
            BulkUploadDraftApprenticeships = new List<BulkUploadAddDraftApprenticeshipRequest>();
        }

        public long ProviderId { get; set; }
        public long? FileUploadLogId { get; set; }
        public bool RplDataExtended { get; set; }
        public UserInfo UserInfo { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> BulkUploadDraftApprenticeships { get; set; }
    }
}
