using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using System;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetReportDataApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid reportId)
    {
        var expectedUrl = $"api/reports/{reportId}/data";

        var actual = new GetReportDataRequest(reportId);

        actual.GetUrl.Should().Be(expectedUrl);
    }
}
