using System.Collections.Generic;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsCommand : IRequest<BulkUploadAddAndApproveDraftApprenticeshipsResult>
    {
        public long ProviderId { get; set; }
        public long? FileUploadLogId { get; set; }
        public UserInfo UserInfo { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> BulkUploadAddAndApproveDraftApprenticeships { get; set; } = new List<BulkUploadAddDraftApprenticeshipRequest>();
    }
}
