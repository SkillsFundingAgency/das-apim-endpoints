using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.EmploymentCheck
{
    public class EmploymentCheckCommand : IRequest
    {
        public Guid CorrelationId { get; set; }
        public string Result { get; set; }
        public DateTime DateChecked { get; set; }

        public EmploymentCheckCommand(Guid correlationId, string result, DateTime dateChecked)
        {
            CorrelationId = correlationId;
            Result = result;
            DateChecked = dateChecked;
        }
    }
}
