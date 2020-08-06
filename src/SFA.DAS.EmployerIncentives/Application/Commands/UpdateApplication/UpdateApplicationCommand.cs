using System;
using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateApplication
{
    public class UpdateApplicationCommand : IRequest<Guid>
    {
        public Guid ApplicationId { get; }
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long[] ApprenticeshipIds { get; }

        public UpdateApplicationCommand(Guid applicationId, long accountId, long accountLegalEntityId, long[] apprenticeshipIds)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            ApprenticeshipIds = apprenticeshipIds;
        }
    }
}