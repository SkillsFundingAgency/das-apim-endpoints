using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetCohortRequest : IGetApiRequest
    {
        public long CohortId { get; set; }
        public string GetUrl => $"api/cohorts/{CohortId}";

        public GetCohortRequest(long cohortId)
        {
            CohortId = cohortId;
        }

    }
}