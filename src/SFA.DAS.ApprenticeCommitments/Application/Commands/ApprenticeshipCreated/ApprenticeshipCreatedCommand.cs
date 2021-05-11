using System;
using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ApprenticeshipCreated
{
    public class ApprenticeshipCreatedCommand : IRequest
    {
        public long EmployerAccountId { get; set; }
        public long ApprenticeshipId { get; set; }
        public DateTime AgreedOn { get; set; }
        public string Email { get; set; }
        public string EmployerName { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
    }
}