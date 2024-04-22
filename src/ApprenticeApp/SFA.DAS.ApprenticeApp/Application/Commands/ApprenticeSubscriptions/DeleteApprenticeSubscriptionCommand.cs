using System;
using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions
{
    public class DeleteApprenticeSubscriptionCommand : IRequest<Unit>
    {
        public Guid ApprenticeId { get; set; }
    }
}