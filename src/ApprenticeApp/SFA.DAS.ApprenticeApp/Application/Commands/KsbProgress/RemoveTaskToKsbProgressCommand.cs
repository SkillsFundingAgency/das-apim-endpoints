using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class RemoveTaskToKsbProgressCommand : IRequest<Unit>
    {
        public long ApprenticeshipId { get; set; }
        public int KsbProgressId { get; set; }
        public int TaskId { get; set; }
    }
}