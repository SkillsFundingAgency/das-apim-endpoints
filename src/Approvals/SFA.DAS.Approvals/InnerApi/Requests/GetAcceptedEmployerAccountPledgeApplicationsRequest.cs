using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAcceptedEmployerAccountPledgeApplicationsRequest : IGetApiRequest
    {
        public long EmployerAccountId { get; }

        public GetAcceptedEmployerAccountPledgeApplicationsRequest(long employerAccountId)
        {
            EmployerAccountId = employerAccountId;
        }
        public string GetUrl => $"applications?ApplicationStatusFilter=Accepted&AccountId={EmployerAccountId}";
    }
}
