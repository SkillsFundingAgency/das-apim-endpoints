using System;
using MediatR;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistration
{
    public class ChangeRegistrationCommand : IRequest
    {
        public long? CommitmentsContinuedApprenticeshipId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
    }
}