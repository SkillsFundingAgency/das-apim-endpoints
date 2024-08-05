using System;
using MediatR;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class AddApprenticeTaskCommand : IRequest<Unit>
    {
        public Guid ApprenticeshipId { get; set; }
        public ApprenticeTaskData Data { get; set; }
    }
}
