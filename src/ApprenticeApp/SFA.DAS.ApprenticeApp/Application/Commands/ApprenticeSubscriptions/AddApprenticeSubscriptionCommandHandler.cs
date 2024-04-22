using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeApp.Application.Events;
using SFA.DAS.ApprenticeApp.Services;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions
{
    public class AddApprenticeSubscriptionCommandHandler : IRequestHandler<AddApprenticeSubscriptionCommand, Unit>
    {
        private readonly ILogger<AddApprenticeSubscriptionCommandHandler> _logger;
        private readonly SubscriptionService _subscriptionService;

        public AddApprenticeSubscriptionCommandHandler
        (
            ILogger<AddApprenticeSubscriptionCommandHandler> logger,
            SubscriptionService subscriptionService
        )
        {
            _logger = logger;
            _subscriptionService = subscriptionService;
        }

        public async Task<Unit> Handle(AddApprenticeSubscriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "AddApprenticeSubscriptionCommand Apprentice:{ApprenticeId}",
                request.ApprenticeId);

            ApprenticeSubscriptionCreateEvent message = new()
            {
                ApprenticeId = request.ApprenticeId,
                Endpoint = request.Endpoint,
                PublicKey = request.PublicKey,
                AuthenticationSecret = request.AuthenticationSecret
            };

            await _subscriptionService.AddApprenticeSubscription(message);

            return Unit.Value;
        }
    }
}