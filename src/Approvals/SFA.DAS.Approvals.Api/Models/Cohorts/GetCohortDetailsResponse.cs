using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;

namespace SFA.DAS.Approvals.Api.Models.Cohorts
{
    public class GetCohortDetailsResponse
    {
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }

        public static implicit operator GetCohortDetailsResponse(GetCohortDetailsQueryResult source)
        {
            return new GetCohortDetailsResponse
            {
                LegalEntityName = source.LegalEntityName,
                ProviderName = source.ProviderName
            };
        }
    }
}
