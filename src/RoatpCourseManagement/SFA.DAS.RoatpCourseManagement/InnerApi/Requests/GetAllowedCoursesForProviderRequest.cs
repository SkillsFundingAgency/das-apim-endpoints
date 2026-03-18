using SFA.DAS.SharedOuterApi.InnerApi;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public record GetAllowedCoursesForProviderRequest(int Ukprn, CourseType CourseType) : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/allowed-courses?courseType={CourseType}";
}
