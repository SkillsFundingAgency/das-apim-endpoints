using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerFeedback
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
