using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class DeleteApprenticeTaskCommand : IRequest<Unit>
    {
        public Guid ApprenticeshipId { get; set; }
        public int TaskId { get; set; }
    }
}