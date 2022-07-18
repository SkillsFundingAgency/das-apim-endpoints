using System;
using MediatR;

namespace SFA.DAS.EmploymentCheck.Application.Commands.RegisterCheck
{
    public class RegisterCheckCommand : IRequest<RegisterCheckResponse>
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