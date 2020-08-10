using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateApplication
{
    public class UpdateApplicationCommand : IRequest
    {
        public Guid ApplicationId { get; }
        public long AccountId { get; }
        public long[] ApprenticeshipIds { get; }

        public UpdateApplicationCommand(Guid applicationId, long accountId, long[] apprenticeshipIds)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            ApprenticeshipIds = apprenticeshipIds;
        }
    }
}