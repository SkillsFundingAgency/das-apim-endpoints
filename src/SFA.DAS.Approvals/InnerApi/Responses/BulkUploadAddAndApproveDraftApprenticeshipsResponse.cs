using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsResponse
    {
        public IEnumerable<BulkUploadAddDraftApprenticeshipsResponse> BulkUploadAddAndApproveDraftApprenticeshipResponse { get; set; }
        public BulkUploadAddAndApproveDraftApprenticeshipsResponse() 
        { 
            BulkUploadAddAndApproveDraftApprenticeshipResponse = new List<BulkUploadAddDraftApprenticeshipsResponse>(); 
        }
    }
}
