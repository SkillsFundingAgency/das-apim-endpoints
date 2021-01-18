using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.InnerApi.Requests
{
    public class GetHealthRequest : IGetApiRequest
    {
        public string GetUrl => "ping";
    }
}