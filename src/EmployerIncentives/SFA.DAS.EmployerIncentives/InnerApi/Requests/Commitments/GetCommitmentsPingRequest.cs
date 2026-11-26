using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments
{
    public class GetCommitmentsPingRequest : IGetApiRequest
    {
        public string GetUrl => "api/ping";
    }
}