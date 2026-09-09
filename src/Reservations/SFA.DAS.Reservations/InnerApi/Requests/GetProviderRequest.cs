using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetProviderRequest : IGetApiRequest
    {
        public int Ukprn { get; set; }
        public string GetUrl => $"api/providers/{Ukprn}";
    }
}