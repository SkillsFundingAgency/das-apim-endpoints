using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
{
    public class GetApprenticeshipKeyRequest : IGetApiRequest
    {
        public string ApprenticeshipHashedId { get; set; }
        public string GetUrl => $"{ApprenticeshipHashedId}/key";
    }
}