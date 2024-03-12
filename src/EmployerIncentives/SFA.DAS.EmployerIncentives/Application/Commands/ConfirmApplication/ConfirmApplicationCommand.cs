using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication
{
    public class ConfirmApplicationCommand : IRequest<Unit>
    {
        public Guid ApplicationId { get; }
        public long AccountId { get; }
        public DateTime DateSubmitted { get; }
        public string SubmittedByEmail { get; }
        public string SubmittedByName { get; }

        public ConfirmApplicationCommand(Guid applicationId, long accountId, DateTime dateSubmitted, string submittedByEmail, string submittedByName)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            DateSubmitted = dateSubmitted;
            SubmittedByEmail = submittedByEmail;
            SubmittedByName = submittedByName;
        }
    }
}