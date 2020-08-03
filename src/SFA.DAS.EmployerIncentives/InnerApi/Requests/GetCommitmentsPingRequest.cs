using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public class GetCommitmentsPingRequest : IGetApiRequest
    {
        public string BaseUrl { get; set; }
        public string Version { get; }
        public string GetUrl => $"{BaseUrl}api/ping";
    }
}