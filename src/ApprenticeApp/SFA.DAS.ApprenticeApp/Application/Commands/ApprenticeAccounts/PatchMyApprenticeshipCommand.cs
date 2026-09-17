using MediatR;
using System;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class PatchMyApprenticeshipCommand : IRequest<bool>
    {
        public Guid ApprenticeId { get; set; }
        public object PatchData { get; set; }
    }
}
