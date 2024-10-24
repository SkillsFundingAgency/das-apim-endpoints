using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Payments.Models.Requests
{
    public class GetLearnersRequest : IGetApiRequest
    {
        private readonly int _ukprn;
        private readonly int _academicYear;
        private readonly int _pageNumber;

        public string GetUrl => $"learners/{_academicYear}?ukprn={_ukprn}&pageNumber={_pageNumber}&pageSize=1000";

        public GetLearnersRequest(int ukprn, int academicYear, int pageNumber)
        {
            _ukprn = ukprn;
            _academicYear = academicYear;
            _pageNumber = pageNumber;
        }
    }
}
