using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.CreateApplication
{
    public class CreateApplicationCommand : IRequest<Unit>
    {
        public Guid ApplicationId { get; }
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long[] ApprenticeshipIds { get; }

        public CreateApplicationCommand(Guid applicationId, long accountId, long accountLegalEntityId, long[] apprenticeshipIds)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            ApprenticeshipIds = apprenticeshipIds;
        }
    }
}