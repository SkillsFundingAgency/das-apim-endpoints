using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

[TestFixture]
internal class WhenBuildingPostReportRequest
{
    [Test, MoqAutoData]
    public void Then_Builds_Correct_Url(PostReportRequest.PostReportRequestData data)
    {
        var actual = new PostReportRequest(data);

        actual.PostUrl.Should().Be("api/reports");
    }

    [Test, MoqAutoData]
    public void Then_OwnerType_Is_Always_Qa(PostReportRequest.PostReportRequestData data)
    {
        var actual = new PostReportRequest(data);

        (actual.Data as PostReportRequest.PostReportRequestData)!.OwnerType.Should().Be("Qa");
    }
}
