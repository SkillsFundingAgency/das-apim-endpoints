using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class RemoveTaskToKsbProgressCommand : IRequest<Unit>
    {
        public Guid ApprenticeshipId { get; set; }
        public int KsbProgressId { get; set; }
        public int TaskId { get; set; }
    }
}