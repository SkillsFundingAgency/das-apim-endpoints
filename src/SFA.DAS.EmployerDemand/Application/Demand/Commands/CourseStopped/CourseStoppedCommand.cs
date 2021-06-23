using System;
using MediatR;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CourseStopped
{
    public class CourseStoppedCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid EmployerDemandId { get; set; }
    }
}

