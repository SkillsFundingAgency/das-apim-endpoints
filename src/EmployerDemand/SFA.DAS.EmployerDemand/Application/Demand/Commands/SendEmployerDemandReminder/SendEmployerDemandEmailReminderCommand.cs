using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.SendEmployerDemandReminder
{
    public class SendEmployerDemandEmailReminderCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid EmployerDemandId { get; set; }
    }
}