using SFA.DAS.Approvals.Application.Cohorts.Queries.GetConfirmEmployerDeclaredStandards;

namespace SFA.DAS.Approvals.Api.Models.Cohorts
{
    public class GetConfirmEmployerDeclaredStandardsResponse
    {
        public bool HasNoDeclaredStandards { get; set; }

        public static implicit operator GetConfirmEmployerDeclaredStandardsResponse(GetConfirmEmployerDeclaredStandardsQueryResult source)
        {
            return new GetConfirmEmployerDeclaredStandardsResponse
            {
                HasNoDeclaredStandards = source.HasNoDeclaredStandards,
            };
        }
    }
}
