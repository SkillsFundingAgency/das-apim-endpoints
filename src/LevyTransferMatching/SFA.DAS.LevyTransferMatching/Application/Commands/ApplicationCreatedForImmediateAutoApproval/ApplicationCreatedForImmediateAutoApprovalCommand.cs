﻿using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationCreatedForImmediateAutoApproval
{
    public class ApplicationCreatedForImmediateAutoApprovalCommand : IRequest<Unit>
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
    }
}
