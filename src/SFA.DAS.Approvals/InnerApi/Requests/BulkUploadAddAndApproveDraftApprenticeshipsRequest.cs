using SFA.DAS.Approvals.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsRequest : SaveDataRequest
    {
        public long ProviderId { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> BulkUploadAddAndApproveDraftApprenticeships { get; set; }
        public BulkCreateReservationsWithNonLevyResult ReservationsWithValidation;
    }
}
