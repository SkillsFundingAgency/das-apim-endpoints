using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments
{
    public class GetCommitmentsPingRequest : IGetApiRequest
    {
        public string GetUrl => "api/ping";
    }
}