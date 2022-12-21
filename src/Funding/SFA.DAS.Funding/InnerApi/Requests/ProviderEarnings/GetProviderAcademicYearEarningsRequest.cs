using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Funding.InnerApi.Requests.ProviderEarnings
{
    public class GetProviderAcademicYearEarningsRequest : IGetApiRequest
    {
        private readonly long _ukprn;

        public GetProviderAcademicYearEarningsRequest(long ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"{_ukprn}/GenerateCSV";
    }
}