using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning
{
    public class GetApprenticeshipKeyRequest : IGetApiRequest
    {
        public string ApprenticeshipHashedId { get; set; }
        public string GetUrl => $"{ApprenticeshipHashedId}/key";
    }
}