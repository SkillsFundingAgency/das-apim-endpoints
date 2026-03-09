using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public record GetAllowedCoursesForProviderRequest(int Ukprn, CourseType CourseType) : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/allowed-courses?courseType={CourseType}";
}
