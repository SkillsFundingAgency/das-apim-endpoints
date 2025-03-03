using MediatR;
using System;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CancelEmployerRequest
{
    public class CancelEmployerRequestCommand : IRequest
    {
        public Guid EmployerRequestId { get; set; }
        public Guid CancelledBy { get; set; }
        public string CancelledByEmail { get; set; }
        public string CancelledByFirstName { get; set; }
        public string CourseLevel { get; set; }
        public string DashboardUrl { get; set; }
    }
}
