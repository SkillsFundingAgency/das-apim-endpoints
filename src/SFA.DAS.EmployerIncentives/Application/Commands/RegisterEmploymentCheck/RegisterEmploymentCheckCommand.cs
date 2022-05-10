using System;
using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck
{
    public class RegisterEmploymentCheckCommand : IRequest<RegisterEmploymentCheckResponse>
    {
        public Guid CorrelationId { get; set; }
        public string CheckType { get; set; }
        public long Uln { get; set; }
        public int ApprenticeshipAccountId { get; set; }
        public long? ApprenticeshipId { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
    }
}
