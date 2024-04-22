using System.Threading.Tasks;
using SFA.DAS.ApprenticeApp.Application.Events;

namespace SFA.DAS.ApprenticeApp.Services
{
    public interface ISubscriptionService
    {
        Task AddApprenticeSubscription(ApprenticeSubscriptionCreateEvent message);
        Task DeleteApprenticeSubscription(ApprenticeSubscriptionDeleteEvent message);
    }
}