using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public record GetProviderAllowedCoursesRequest(int Ukprn, CourseType CourseType) : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/allowed-courses?courseType={CourseType}";
}
