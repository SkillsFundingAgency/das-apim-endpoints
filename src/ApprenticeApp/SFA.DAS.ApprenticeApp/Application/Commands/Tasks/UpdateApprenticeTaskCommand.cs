using System;
using MediatR;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class UpdateApprenticeTaskCommand : IRequest<Unit>
    {
        public long ApprenticeshipId { get; set; }
        public int TaskId { get; set; }
        public ApprenticeTaskData Data { get; set; }
    }
}