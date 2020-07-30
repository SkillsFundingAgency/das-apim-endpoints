using System;
using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Command.CreateApplication
{
    public class CreateApplicationCommand : IRequest<Guid>
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
            ApprenticeshipIds = ApprenticeshipIds;
        }
    }
}