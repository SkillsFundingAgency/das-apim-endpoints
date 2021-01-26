using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis
{
    public class GetHealthRequest : IGetApiRequest
    {
        public string GetUrl => "ping";
    }
}