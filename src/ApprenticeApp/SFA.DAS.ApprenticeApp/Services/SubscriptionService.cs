﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.PushNotifications.Messages.Commands;

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
      
        public async Task AddApprenticeSubscription(AddWebPushSubscriptionCommand message)
        {
            try
            {
                _logger.LogInformation($"Processing {nameof(AddWebPushSubscriptionCommand)} to publish to NServiceBus");

                await _messageSession.Publish(message);

                _logger.LogInformation($"Finished processing {nameof(AddWebPushSubscriptionCommand)} to publish to NServiceBus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process {eventBody}", message);
                throw;
            }
        }

        public async Task DeleteApprenticeSubscription(RemoveWebPushSubscriptionCommand message)
        {
            try
            {
                _logger.LogInformation($"Processing {nameof(RemoveWebPushSubscriptionCommand)} to publish to NServiceBus");

                await _messageSession.Publish(message);

                _logger.LogInformation($"Finished processing {nameof(RemoveWebPushSubscriptionCommand)} to publish to NServiceBus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to process {eventBody}", message);
                throw;
            }
        }
    }
}