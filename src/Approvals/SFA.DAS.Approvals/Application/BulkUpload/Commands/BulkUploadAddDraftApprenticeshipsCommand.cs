using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddDraftApprenticeshipsCommand : IRequest<GetBulkUploadAddDraftApprenticeshipsResult>
    {
        public long ProviderId { get; set; }
        public long? FileUploadLogId { get; set; }
        public UserInfo UserInfo { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> BulkUploadAddDraftApprenticeships { get; set; } = new List<BulkUploadAddDraftApprenticeshipRequest>();
    }
}
