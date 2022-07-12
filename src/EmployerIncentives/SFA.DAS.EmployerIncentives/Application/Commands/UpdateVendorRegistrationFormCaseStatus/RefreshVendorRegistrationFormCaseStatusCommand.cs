using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class RefreshVendorRegistrationFormCaseStatusCommand : IRequest<DateTime>
    {
        public RefreshVendorRegistrationFormCaseStatusCommand(DateTime from)
        {
            FromDateTime = from;
        }

        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
    }
}