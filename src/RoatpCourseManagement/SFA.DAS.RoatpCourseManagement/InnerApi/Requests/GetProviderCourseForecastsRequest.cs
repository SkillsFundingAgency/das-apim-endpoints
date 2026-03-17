using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public record GetProviderCourseForecastsRequest(int Ukprn, string LarsCode) : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/courses/{LarsCode}/forecasts";
}
