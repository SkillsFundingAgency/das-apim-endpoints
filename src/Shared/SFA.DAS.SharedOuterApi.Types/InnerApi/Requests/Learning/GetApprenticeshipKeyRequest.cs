using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Learning
{
    public class GetApprenticeshipKeyRequest : IGetApiRequest
    {
        public string ApprenticeshipHashedId { get; set; }
        public string GetUrl => $"{ApprenticeshipHashedId}/key";
    }
}