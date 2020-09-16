using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class RefreshVendorRegistrationFormCaseStatusCommand : IRequest
    {
        public RefreshVendorRegistrationFormCaseStatusCommand(DateTime from, DateTime to)
        {
            FromDateTime = from;
            ToDateTime = to;
        }

        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
    }
}