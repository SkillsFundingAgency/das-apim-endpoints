using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.InnerApi.Requests
{
    public class GetProviderAccountLegalEntities : IGetApiRequest
    {
        private readonly int _ukprn;

        public GetProviderAccountLegalEntities(int ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"/accountproviderlegalentities?ukprn={_ukprn}";
    }
}