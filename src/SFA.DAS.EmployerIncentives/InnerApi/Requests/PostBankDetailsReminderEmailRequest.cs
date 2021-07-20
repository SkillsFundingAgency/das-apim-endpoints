using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class PostBankDetailsReminderEmailRequest : IPostApiRequest
    {
        public string PostUrl => "api/EmailCommand/bank-details-reminder";

        public object Data { get; set; }
    }
}
