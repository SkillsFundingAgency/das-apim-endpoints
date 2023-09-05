using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests
{
    public class GetProviderRequest : IGetApiRequest
    {
        private readonly long _ukprn;

        public GetProviderRequest(long ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"api/providers/{_ukprn}";
    }
}
