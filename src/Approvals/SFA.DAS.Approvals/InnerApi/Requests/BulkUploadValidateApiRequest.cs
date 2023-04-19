using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Types;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class BulkUploadValidateApiRequest : SaveDataRequest
    {
        public long ProviderId { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> CsvRecords { get; set; }
        public BulkReservationValidationResults BulkReservationValidationResults { get; set; }
        public ProviderStandardsData ProviderStandardsData { get; set; }
    }
}