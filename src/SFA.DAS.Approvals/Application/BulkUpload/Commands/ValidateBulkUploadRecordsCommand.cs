using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class ValidateBulkUploadRecordsCommand :IRequest
    {
        public long ProviderId { get; set; }
        public List<BulkUploadAddDraftApprenticeshipRequest> CsvRecords { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
