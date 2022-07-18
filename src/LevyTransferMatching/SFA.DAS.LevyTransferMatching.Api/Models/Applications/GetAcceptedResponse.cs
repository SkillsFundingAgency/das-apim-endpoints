using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetAccepted;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetAcceptedResponse
    {
        public string EmployerAccountName { get; set; }
        public int OpportunityId { get; set; }

        public static implicit operator GetAcceptedResponse(GetAcceptedResult result)
        {
            return new GetAcceptedResponse()
            {
                EmployerAccountName = result.EmployerAccountName,
                OpportunityId = result.OpportunityId,
            };
        }
    }
}