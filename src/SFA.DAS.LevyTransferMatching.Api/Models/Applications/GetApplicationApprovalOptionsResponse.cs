using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationApprovalOptions;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApplicationApprovalOptionsResponse
    {
        public string EmployerAccountName { get; set; }

        public static implicit operator GetApplicationApprovalOptionsResponse(GetApplicationApprovalOptionsResult result)
        {
            return new GetApplicationApprovalOptionsResponse()
            {
                EmployerAccountName = result.EmployerAccountName,
            };
        }
    }
}