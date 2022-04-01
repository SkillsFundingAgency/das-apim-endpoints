﻿using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddAndApproveDraftApprenticeshipsCommand : IRequest<BulkUploadAddAndApproveDraftApprenticeshipsResponse>
    {
        public BulkUploadAddAndApproveDraftApprenticeshipsCommand()
        {
            BulkUploadAddAndApproveDraftApprenticeships = new List<BulkUploadAddDraftApprenticeshipRequest>();
        }

        public long ProviderId { get; set; }
        public UserInfo UserInfo { get; set; }
        public IEnumerable<BulkUploadAddDraftApprenticeshipRequest> BulkUploadAddAndApproveDraftApprenticeships { get; set; }
    }
}
