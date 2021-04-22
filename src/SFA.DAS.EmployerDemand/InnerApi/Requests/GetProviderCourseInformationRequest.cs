using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class GetProviderCourseInformationRequest : IGetApiRequest
    {
        private readonly int _ukprn;
        private readonly int _courseId;

        public GetProviderCourseInformationRequest(int ukprn, int courseId)
        {
            _ukprn = ukprn;
            _courseId = courseId;
        }

        public string GetUrl => $"api/Courses/{_courseId}/providers/{_ukprn}";
    }
}