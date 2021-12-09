using MediatR;
using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApproval
{
    public class CreateApprovalCommand : IRequest<CreateApprovalResponse>
    {
        public long EmployerAccountId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
        public string Email { get; set; }
        public string EmployerName { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
    }
}