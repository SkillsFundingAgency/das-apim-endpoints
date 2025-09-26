using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests
{
    public class GetAllCoursesRequest : IGetApiRequest
    {
        public string GetUrl => $"providers/{Ukprn}/courses?excludeCoursesWithoutLocation={ExcludeInvalidCourses}";
        public int Ukprn { get; }
        public bool ExcludeInvalidCourses { get; }

        public GetAllCoursesRequest(int ukprn)
        {
            Ukprn = ukprn;
            ExcludeInvalidCourses = false;
        }
    }
}