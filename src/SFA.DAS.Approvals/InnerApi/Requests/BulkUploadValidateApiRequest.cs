using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkUploadValidateApiRequest : SaveDataRequest
    {
        public long ProviderId { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> CsvRecords { get; set; }
    }
}
