using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApprovalOptions;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationApprovalOptionsResponse
    {
        public string EmployerAccountName { get; set; }
        public string ApplicationStatus { get; set; }

        public static implicit operator GetApplicationApprovalOptionsResponse(GetApplicationApprovalOptionsQueryResult result)
        {
            return new GetApplicationApprovalOptionsResponse()
            {
                EmployerAccountName = result.EmployerAccountName,
                ApplicationStatus = result.ApplicationStatus
            };
        }
    }
}
