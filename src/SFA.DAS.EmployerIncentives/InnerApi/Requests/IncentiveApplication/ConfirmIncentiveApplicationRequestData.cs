using System;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication
{
    public class ConfirmIncentiveApplicationRequestData
    {
        public ConfirmIncentiveApplicationRequestData(Guid applicationId, long accountId, DateTime dateSubmitted, string submittedBy)
        {
            IncentiveApplicationId = applicationId;
            AccountId = accountId;
            DateSubmitted = dateSubmitted;
            SubmittedBy = submittedBy;
        }

        public Guid IncentiveApplicationId { get; set; }
        public long AccountId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string SubmittedBy { get; set; }
    }
}
