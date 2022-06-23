using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class SendBankDetailsRepeatReminderEmailsRequest
    {
        public SendBankDetailsRepeatReminderEmailsRequest(DateTime applicationCutOffDate)
        {
            ApplicationCutOffDate = applicationCutOffDate;
        } 

        public DateTime ApplicationCutOffDate { get; private set; }
    }
}
