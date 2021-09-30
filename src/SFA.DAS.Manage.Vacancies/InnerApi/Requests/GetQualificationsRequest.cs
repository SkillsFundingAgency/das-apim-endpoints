using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class GetQualificationsRequest : IGetApiRequest
    {
        private readonly int _ukprn;

        public GetQualificationsRequest(int ukprn)
        {
            _ukprn = ukprn;
        }

        public string GetUrl => $"accountproviderlegalentities?ukprn={_ukprn}&operations=1&operations=2";
    }
}