using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.InnerApi.Requests
{
    public class GetProviderRequest : IGetApiRequest
    {
        public int Ukprn { get; set; }
        public string GetUrl => $"api/providers/{Ukprn}";
    }
}