using SFA.DAS.Approvals.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsRequest
    {
        public BulkUploadAddAndApproveDraftApprenticeshipsRequest()
        {
            BulkUploadAddAndApproveDraftApprenticeships = new List<BulkUploadAddDraftApprenticeshipRequest>();
        }

        public long ProviderId { get; set; }
        public UserInfo UserInfo { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> BulkUploadAddAndApproveDraftApprenticeships { get; set; }
    }
}
