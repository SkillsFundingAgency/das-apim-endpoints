using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetDeclined;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetDeclinedResponse
    {
        public string EmployerAccountName { get; set; }
        public int OpportunityId { get; set; }

        public static implicit operator GetDeclinedResponse(GetDeclinedResult result)
        {
            return new GetDeclinedResponse()
            {
                EmployerAccountName = result.EmployerAccountName,
                OpportunityId = result.OpportunityId,
            };
        }
    }
}