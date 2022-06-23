using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail<T>(T emailRequest) where T: IPostApiRequest;
        Task TriggerBankRepeatReminderEmails(SendBankDetailsRepeatReminderEmailsRequest sendBankDetailsRepeatReminderEmailsRequest);
    }
}
