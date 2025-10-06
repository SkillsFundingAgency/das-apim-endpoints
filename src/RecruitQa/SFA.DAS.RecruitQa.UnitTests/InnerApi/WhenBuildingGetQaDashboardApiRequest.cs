using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

[TestFixture]
internal class WhenBuildingGetQaDashboardApiRequest
{
    [Test, MoqAutoData]
    public void Then_Builds_Correct_Request(
        GetQaDashboardApiRequest request)
    {
        // act
        var actual = new GetQaDashboardApiRequest();
        // assert
        actual.GetUrl.Should().Be("api/vacancyreviews/qa/dashboard");
    }
}