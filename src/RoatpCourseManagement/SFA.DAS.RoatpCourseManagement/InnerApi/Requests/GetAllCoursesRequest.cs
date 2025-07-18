using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetAllCoursesRequest : IGetApiRequest
    {
        public string GetUrl => $"providers/{Ukprn}/courses?excludeCoursesWithoutLocation={ExcludeCoursesWithoutLocation}";
        public int Ukprn { get; }
        public bool ExcludeCoursesWithoutLocation { get; }

        public GetAllCoursesRequest(int ukprn)
        {
            Ukprn = ukprn;
            ExcludeCoursesWithoutLocation = false;
        }
    }
}