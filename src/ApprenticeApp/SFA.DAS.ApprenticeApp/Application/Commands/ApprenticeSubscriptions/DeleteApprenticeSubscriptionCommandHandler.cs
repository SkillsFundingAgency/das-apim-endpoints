using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeApp.Application.Events;
using SFA.DAS.ApprenticeApp.Services;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeSubscriptions
{
    public class DeleteApprenticeSubscriptionCommandHandler : IRequestHandler<DeleteApprenticeSubscriptionCommand, Unit>
    {
        private readonly ILogger<DeleteApprenticeSubscriptionCommandHandler> _logger;
        private readonly SubscriptionService _subscriptionService;

        public DeleteApprenticeSubscriptionCommandHandler
        (
            ILogger<DeleteApprenticeSubscriptionCommandHandler> logger,
            SubscriptionService subscriptionService
        )
        {
            _logger = logger;
            _subscriptionService = subscriptionService;
        }

        public async Task<Unit> Handle(DeleteApprenticeSubscriptionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "DeleteApprenticeSubscriptionCommand Apprentice:{ApprenticeId}",
                request.ApprenticeId);

            ApprenticeSubscriptionDeleteEvent message = new()
            {
                ApprenticeId = request.ApprenticeId
            };

            await _subscriptionService.DeleteApprenticeSubscription(message);

            return Unit.Value;
        }
    }
}
