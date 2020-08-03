using System;
using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication
{
    public class ConfirmApplicationCommand : IRequest
    {
        public Guid ApplicationId { get; }
        public long AccountId { get; }
        public DateTime DateSubmitted { get; }

        public ConfirmApplicationCommand(Guid applicationId, in long accountId, in DateTime dateSubmitted)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            DateSubmitted = dateSubmitted;
        }
    }
}