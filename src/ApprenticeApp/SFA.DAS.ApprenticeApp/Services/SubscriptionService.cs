using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.ApprenticeApp.Application.Events;

namespace SFA.DAS.ApprenticeApp.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ILogger<SubscriptionService> _logger;
        private readonly IMessageSession _messageSession;

        public SubscriptionService(
            ILogger<SubscriptionService> logger,
            IMessageSession messageSession
            )
        {
            _logger = logger;
            _messageSession = messageSession;
        }
      
        public async Task AddApprenticeSubscription(ApprenticeSubscriptionCreateEvent message)
        {
            try
            {
                _logger.LogInformation($"Processing {nameof(ApprenticeSubscriptionCreateEvent)} to publish to NServiceBus");

                await _messageSession.Publish(message);

                _logger.LogInformation($"Finished processing {nameof(ApprenticeSubscriptionCreateEvent)} to publish to NServiceBus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process {eventBody}", message);
                throw;
            }
        }

        public async Task DeleteApprenticeSubscription(ApprenticeSubscriptionDeleteEvent message)
        {
            try
            {
                _logger.LogInformation($"Processing {nameof(ApprenticeSubscriptionDeleteEvent)} to publish to NServiceBus");

                await _messageSession.Publish(message);

                _logger.LogInformation($"Finished processing {nameof(ApprenticeSubscriptionDeleteEvent)} to publish to NServiceBus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process {eventBody}", message);
                throw;
            }
        }
    }
}