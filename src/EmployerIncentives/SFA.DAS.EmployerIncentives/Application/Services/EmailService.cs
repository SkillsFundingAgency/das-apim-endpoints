using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public EmailService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task TriggerBankRepeatReminderEmails(SendBankDetailsRepeatReminderEmailsRequest sendBankDetailsRepeatReminderEmailsRequest)
        {
            var request = new PostBankDetailsRepeatReminderEmailsRequest { Data = sendBankDetailsRepeatReminderEmailsRequest };

            await _client.PostWithResponseCode<SendBankDetailsRepeatReminderEmailsRequest>(request, false);
        }

        public async Task SendEmail<T>(T emailRequest) where T : IPostApiRequest
        {
            await _client.PostWithResponseCode<T>(emailRequest, false);
        }
    }
}
