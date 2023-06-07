using SFA.DAS.Approvals.Application.Cohorts.Queries.GetConfirmEmployer;

namespace SFA.DAS.Approvals.Api.Models.Cohorts
{
    public class GetConfirmEmployerResponse
    {
        public bool HasNoDeclaredStandards { get; set; }

        public static implicit operator GetConfirmEmployerResponse(GetConfirmEmployerResult source)
        {
            return new GetConfirmEmployerResponse
            {
                HasNoDeclaredStandards = source.HasNoDeclaredStandards,
            };
        }
    }
}
