using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions
{
    public class RemoveApprenticeSubscriptionCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
        public string Endpoint { get; set; }
    }
}