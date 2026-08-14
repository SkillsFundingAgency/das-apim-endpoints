using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.SelectMultiple.Commands
{
    public class ValidateSelectMultipleLearnerRecordsCommand : IRequest
    {
        public long ProviderId { get; set; }
        public long? FileUploadLogId { get; set; }
        public List<BulkUploadAddDraftApprenticeshipRequest> CsvRecords { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
