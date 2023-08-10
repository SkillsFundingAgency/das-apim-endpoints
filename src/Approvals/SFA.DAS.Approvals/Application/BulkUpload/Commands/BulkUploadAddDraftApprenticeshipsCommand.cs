using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddDraftApprenticeshipsCommand : IRequest<GetBulkUploadAddDraftApprenticeshipsResult>
    {
        public BulkUploadAddDraftApprenticeshipsCommand()
        {
            BulkUploadAddDraftApprenticeships = new List<BulkUploadAddDraftApprenticeshipRequest>();
        }

        public long ProviderId { get; set; }
        public bool RplDataExtended { get; set; }
        public UserInfo UserInfo { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> BulkUploadAddDraftApprenticeships { get; set; }
    }
}
