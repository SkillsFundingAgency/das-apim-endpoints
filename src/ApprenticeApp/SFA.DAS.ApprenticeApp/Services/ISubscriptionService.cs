using System.Threading.Tasks;
using SFA.DAS.PushNotifications.Messages.Commands;

namespace SFA.DAS.ApprenticeApp.Services
{
    public interface ISubscriptionService
    {
        Task AddApprenticeSubscription(AddWebPushSubscriptionCommand message);
        Task RemoveApprenticeSubscription(RemoveWebPushSubscriptionCommand message);
    }
}