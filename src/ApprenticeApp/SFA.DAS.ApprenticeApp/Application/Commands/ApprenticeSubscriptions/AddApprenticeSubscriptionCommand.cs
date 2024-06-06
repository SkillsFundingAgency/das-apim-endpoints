using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions
{
    public class AddApprenticeSubscriptionCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
        public string Endpoint { get; set; }
        public string PublicKey { get; set; }
        public string AuthenticationSecret { get; set; }
    }
}