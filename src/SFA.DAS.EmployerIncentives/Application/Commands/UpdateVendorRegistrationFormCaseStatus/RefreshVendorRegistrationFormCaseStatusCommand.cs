using MediatR;
using System;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class RefreshVendorRegistrationFormCaseStatusCommand : IRequest
    {
        public RefreshVendorRegistrationFormCaseStatusCommand(DateTime from)
        {
            FromDateTime = from;
        }

        public DateTime FromDateTime { get; set; }
    }
}