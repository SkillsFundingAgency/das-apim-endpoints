using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetReportByIdRequest
{
    [Test, AutoData]
    public void Then_Builds_Correct_Url(Guid reportId)
    {
        var actual = new GetReportByIdRequest(reportId);
        
        actual.GetUrl.Should().Be($"api/reports/{reportId}");
    }
}