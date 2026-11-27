using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

internal class WhenBuildingGetReportsRequest
{
    [Test]
    public void Then_Builds_Correct_Url()
    {
        var actual = new GetReportsRequest();

        actual.GetUrl.Should().Be("api/reports?ownerType=Qa");
    }
}