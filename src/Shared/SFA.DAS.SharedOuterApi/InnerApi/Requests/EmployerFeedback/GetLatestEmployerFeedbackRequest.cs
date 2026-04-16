using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFeedback
{
    public class GetLatestEmployerFeedbackRequest : IGetApiRequest
    {
        public long AccountId { get; set; }
        public Guid UserRef { get; set; }

        public GetLatestEmployerFeedbackRequest(long accountId, Guid userRef)
        {
            AccountId = accountId;
            UserRef = userRef;
        }

        public string GetUrl => $"api/employerfeedback?accountid={AccountId}&userref={UserRef}";
    }
}
