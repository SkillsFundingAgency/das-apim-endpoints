using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Payments.Models.Requests
{
    public class GetLearnersRequest : IGetApiRequest
    {
        private readonly string _ukprn;
        private readonly short _academicYear;
        private readonly int _pageNumber;

        public string GetUrl => $"learners/{_academicYear}?ukprn={_ukprn}&fundModel=36&progType=-1&standardCode=-1&pageNumber={_pageNumber}&pageSize=1000";

        public GetLearnersRequest(string ukprn, short academicYear, int pageNumber)
        {
            _ukprn = ukprn;
            _academicYear = academicYear;
            _pageNumber = pageNumber;
        }
    }
}
