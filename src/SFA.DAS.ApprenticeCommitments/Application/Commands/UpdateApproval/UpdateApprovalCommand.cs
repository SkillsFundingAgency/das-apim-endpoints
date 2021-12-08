using System;
using MediatR;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateApproval
{
    public class UpdateApprovalCommand : IRequest
    {
        public long? CommitmentsContinuedApprenticeshipId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
    }
}