using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class SendReminderEmailRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public SendReminderEmailRequest(ReminderEmail reminderEmail)
        {
            Data = reminderEmail;
        }
        public string PostUrl => "/api/student-triage-data/reminder";
    }
}