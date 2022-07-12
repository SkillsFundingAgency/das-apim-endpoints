using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class ConfirmIncentiveApplicationRequestData
    {
        public ConfirmIncentiveApplicationRequestData(Guid applicationId, long accountId, DateTime dateSubmitted, string submittedByEmail, string submittedByName)
        {
            IncentiveApplicationId = applicationId;
            AccountId = accountId;
            DateSubmitted = dateSubmitted;
            SubmittedByEmail = submittedByEmail;
            SubmittedByName = submittedByName;
        }

        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string SubmittedByEmail { get; set; }
        public string SubmittedByName { get; set; }
    }
}
