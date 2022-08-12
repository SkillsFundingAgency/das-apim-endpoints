using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command
{
    public class ValidateDraftApprenticeshipDetailsCommand : IRequest
    {
        public ValidateDraftApprenticeshipRequest DraftApprenticeshipRequest { get; set; }

    }
}
