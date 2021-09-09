using MediatR;
using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistration
{
    public class CreateRegistrationCommand : IRequest<CreateRegistrationResponse>
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