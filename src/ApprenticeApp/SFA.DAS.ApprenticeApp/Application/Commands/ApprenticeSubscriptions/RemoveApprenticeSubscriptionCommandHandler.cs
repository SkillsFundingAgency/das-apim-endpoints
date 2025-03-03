using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.PushNotifications.Messages.Commands;
using SFA.DAS.ApprenticeApp.Services;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions
{
    public class RemoveApprenticeSubscriptionCommandHandler : IRequestHandler<RemoveApprenticeSubscriptionCommand, Unit>
    {
        private readonly ILogger<RemoveApprenticeSubscriptionCommandHandler> _logger;
        private readonly SubscriptionService _subscriptionService;

        public RemoveApprenticeSubscriptionCommandHandler
        (
            ILogger<RemoveApprenticeSubscriptionCommandHandler> logger,
            SubscriptionService subscriptionService
        )
        {
            _logger = logger;
            _subscriptionService = subscriptionService;
        }

        public async Task<Unit> Handle(RemoveApprenticeSubscriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "RemoveApprenticeSubscriptionCommand Apprentice:{ApprenticeId}",
                request.ApprenticeId);

            RemoveWebPushSubscriptionCommand message = new()
            {
                ApprenticeId = request.ApprenticeId,
                Endpoint = request.Endpoint
            };

            await _subscriptionService.RemoveApprenticeSubscription(message);

            return Unit.Value;
        }
    }
}
