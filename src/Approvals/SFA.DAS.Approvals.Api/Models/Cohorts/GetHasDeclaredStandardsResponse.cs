using SFA.DAS.Approvals.Application.Cohorts.Queries.GetHasDeclaredStandards;

namespace SFA.DAS.Approvals.Api.Models.Cohorts
{
    public class GetHasDeclaredStandardsResponse
    {
        public bool HasNoDeclaredStandards { get; set; }

        public static implicit operator GetHasDeclaredStandardsResponse(GetHasDeclaredStandardsQueryResult source)
        {
            return new GetHasDeclaredStandardsResponse
            {
                HasNoDeclaredStandards = source.HasNoDeclaredStandards,
            };
        }
    }
}
