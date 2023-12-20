using SFA.DAS.Approvals.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class BulkUploadValidateApimRequest
    {
        public long ProviderId { get; set; }
        public bool RplDataExtended { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> CsvRecords { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
