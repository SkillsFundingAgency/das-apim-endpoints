using MediatR;
using System;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeAccounts
{
    public class ApprenticePatchCommand : IRequest<Unit>
    {
        public object Patch { get; set; }
        public Guid ApprenticeId { get; set; }
    }
}