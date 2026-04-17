using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

[TestFixture]
internal class WhenBuildingGetGenerateQaReportRequest
{
    [Test, MoqAutoData]
    public void Then_Builds_Correct_Url(Guid reportId)
    {
        var actual = new GetGenerateQaReportRequest(reportId);

        actual.GetUrl.Should().Be($"api/reports/generate-qa/{reportId}");
    }
}
