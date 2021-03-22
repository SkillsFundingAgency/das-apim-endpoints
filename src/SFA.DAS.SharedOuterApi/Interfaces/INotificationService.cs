using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface INotificationService
    {
        Task Send(EmailTemplateArguments email);
    }
}