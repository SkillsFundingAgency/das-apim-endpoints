using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public EmailService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }
        
        public async Task SendBankDetailRequiredEmail(long accountId, SendBankDetailsEmailRequest sendBankDetailsEmailRequest)
        {
            var request = new PostBankDetailsRequiredEmailRequest(accountId)
                { Data = sendBankDetailsEmailRequest };

            await _client.Post<SendBankDetailsEmailRequest>(request);
        }

        public async Task SendBankDetailReminderEmail(long accountId, SendBankDetailsEmailRequest sendBankDetailsEmailRequest)
        {
            var request = new PostBankDetailsReminderEmailRequest(accountId)
                { Data = sendBankDetailsEmailRequest };

            await _client.Post<SendBankDetailsEmailRequest>(request);
        }

        public async Task SendBankDetailsRepeatReminderEmails(SendBankDetailsRepeatReminderEmailsRequest sendBankDetailsRepeatReminderEmailsRequest)
        {
            var request = new PostBankDetailsRepeatReminderEmailsRequest { Data = sendBankDetailsRepeatReminderEmailsRequest };

            await _client.Post<SendBankDetailsRepeatReminderEmailsRequest>(request);
        }
    }
}
