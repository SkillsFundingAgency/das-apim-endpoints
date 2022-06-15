using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostBankDetailsRepeatReminderEmailsRequest : IPostApiRequest
    {
        public long AccountId { get; private set; }
        public string PostUrl => "api/EmailCommand/bank-details-repeat-reminders";

        public object Data { get; set; }
    }
}
