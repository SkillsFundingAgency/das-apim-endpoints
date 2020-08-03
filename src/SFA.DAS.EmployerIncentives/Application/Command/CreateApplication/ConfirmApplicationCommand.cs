using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Command.CreateApplication
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