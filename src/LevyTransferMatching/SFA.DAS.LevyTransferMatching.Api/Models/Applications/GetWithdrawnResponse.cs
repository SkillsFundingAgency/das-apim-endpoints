using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawn;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetWithdrawnResponse
    {
        public string EmployerAccountName { get; set; }
        public int OpportunityId { get; set; }

        public static implicit operator GetWithdrawnResponse(GetWithdrawnQueryResult result)
        {
            return new GetWithdrawnResponse()
            {
                EmployerAccountName = result.EmployerAccountName,
                OpportunityId = result.OpportunityId,
            };
        }
    }
}