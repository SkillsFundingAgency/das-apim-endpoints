using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.NServiceBus.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEventPublisher _messageSession;

        public NotificationService(IEventPublisher messageSession)
        {
            _messageSession = messageSession;
        }

        public Task Send(SendEmailCommand email)
        {
            return _messageSession.Publish(email);
        }
    }
}