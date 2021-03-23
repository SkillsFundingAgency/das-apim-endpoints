using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMessageSession _messageSession;

        public NotificationService(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public Task Send(EmailTemplateArguments email)
        {
            return _messageSession.Send(email);
        }
    }
}