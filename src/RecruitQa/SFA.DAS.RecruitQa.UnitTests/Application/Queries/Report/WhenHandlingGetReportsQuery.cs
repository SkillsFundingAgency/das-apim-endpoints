using SFA.DAS.RecruitQa.Application.Report.Queries.GetReports;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using ReportModel = SFA.DAS.RecruitQa.Domain.Reports.Report;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.Report;

public class WhenHandlingGetReportsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Reports_Returned(
        List<ReportModel> reports,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetReportsQueryHandler handler)
    {
        recruitApiClient
            .Setup(x => x.Get<List<ReportModel>>(
                It.Is<GetReportsRequest>(r => r.GetUrl == "api/reports?ownerType=Qa")))
            .ReturnsAsync(reports);

        var actual = await handler.Handle(new GetReportsQuery(), CancellationToken.None);

        actual.Reports.Should().BeEquivalentTo(reports);
    }

    [Test, MoqAutoData]
    public async Task Then_Empty_List_Returned_When_Api_Returns_Null(
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetReportsQueryHandler handler)
    {
        recruitApiClient
            .Setup(x => x.Get<List<ReportModel>>(It.IsAny<GetReportsRequest>()))
            .ReturnsAsync((List<ReportModel>?)null);

        var actual = await handler.Handle(new GetReportsQuery(), CancellationToken.None);

        actual.Reports.Should().BeEmpty();
    }
}
