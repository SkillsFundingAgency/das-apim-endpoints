using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.BusinessMetrics;
using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetVacancyMetricsRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(DateTime startDate, DateTime endDate)
    {
        var actual = new GetVacancyMetricsRequest(startDate, endDate);

        actual.GetUrl.Should().Be($"api/vacancies/metrics?startDate={startDate:O}&endDate={endDate:O}");
    }
}