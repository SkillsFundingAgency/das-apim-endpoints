using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ProviderFeedback.Application.InnerApi.Requests
{
    public class GetEmployerFeedbackForAcademicYearRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerfeedbackresult/{_ukprn}/annual/{_year}";
        private long _ukprn { get; }
        public string _year { get; }
        public GetEmployerFeedbackForAcademicYearRequest(long ukprn,string year)
        {
            _ukprn = ukprn;
            _year = year;
        }
    }

}