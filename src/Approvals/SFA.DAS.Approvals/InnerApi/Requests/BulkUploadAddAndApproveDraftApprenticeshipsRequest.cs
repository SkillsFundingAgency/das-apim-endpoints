﻿using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsRequest : SaveDataRequest
    {
        public long ProviderId { get; set; }
        public long? LogId { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipExtendedRequest> BulkUploadAddAndApproveDraftApprenticeships { get; set; }
    }
}
