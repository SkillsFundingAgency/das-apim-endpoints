using System.Threading.Tasks;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Models.Messages;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface INotificationService
    {
        Task Send(SendEmailCommand email);
    }
}