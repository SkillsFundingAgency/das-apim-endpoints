using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Cohorts
{
    public class SendCohortRequest : IPostApiRequest
    {
        public long CohortId { get; }
        public string PostUrl => $"api/cohorts/{CohortId}/approve";
        public object Data { get; set; }

        public SendCohortRequest(long cohortId)
        {
            CohortId = cohortId;
        }

        public class Body : SaveDataRequest
        {
            public Party RequestingParty { get; set; }
            public string Message { get; set; }
        }
    }
}