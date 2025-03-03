using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Requests
{
    public class GetApprenticeFeedbackForAcademicYearRequest : IGetApiRequest
    {
        public string GetUrl => $"api/apprenticefeedbackresult/{_ukprn}/annual/{_year}";
        private long _ukprn { get; }
        public string _year { get; }
        public GetApprenticeFeedbackForAcademicYearRequest(long ukprn,string year)
        {
            _ukprn = ukprn;
            _year = year;
        }
    }

}