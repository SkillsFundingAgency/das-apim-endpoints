using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerDemand.Application.Demand.Services
{
    public class NotificationService
    {
        private readonly IMessageSession _messageSession;

        public NotificationService(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public Task Send(SendEmailCommand email)
        {
            return _messageSession.Send(email);
        }
    }
}