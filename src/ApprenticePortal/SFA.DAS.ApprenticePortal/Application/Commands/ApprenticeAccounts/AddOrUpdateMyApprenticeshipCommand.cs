using System;
using MediatR;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeAccounts
{
    public class AddOrUpdateMyApprenticeshipCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
    }
}
