using System;
using MediatR;

namespace SFA.DAS.ApprenticePortal.Application.ApprenticeAccounts.Commands
{
    public class AddOrUpdateMyApprenticeshipCommand : IRequest
    {
        public Guid ApprenticeId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
    }
}
