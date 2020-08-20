using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication
{
    public class ConfirmApplicationCommand : IRequest
    {
        public Guid ApplicationId { get; }
        public long AccountId { get; }
        public DateTime DateSubmitted { get; }
        public string SubmittedBy { get; }

        public ConfirmApplicationCommand(Guid applicationId, long accountId, DateTime dateSubmitted, string submittedBy)
        {
            ApplicationId = applicationId;
            AccountId = accountId;
            DateSubmitted = dateSubmitted;
            SubmittedBy = submittedBy;
        }
    }
}