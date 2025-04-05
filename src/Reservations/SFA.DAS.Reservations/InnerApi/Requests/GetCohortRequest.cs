using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetCohortRequest(long cohortId) : IGetApiRequest
    {
        public long CohortId { get; set; } = cohortId;
        public string GetUrl => $"api/cohorts/{CohortId}";
    }
}