using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitQa.Application.Report.Queries.GetReports;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.Report;

public class WhenHandlingGetReportsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Reports_Returned(
        List<Recruit.Contracts.ApiResponses.Report> reports,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        GetReportsQueryHandler handler)
    {
        recruitApiClient
            .Setup(x => x.Get<List<Recruit.Contracts.ApiResponses.Report>>(
                It.IsAny<GetReportsApiRequest>()))
            .ReturnsAsync(reports);

        var actual = await handler.Handle(new GetReportsQuery(), CancellationToken.None);

        actual.Reports.Should().BeEquivalentTo(reports);
    }

    [Test, MoqAutoData]
    public async Task Then_Empty_List_Returned_When_Api_Returns_Null(
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        GetReportsQueryHandler handler)
    {
        recruitApiClient
            .Setup(x => x.Get<List<Recruit.Contracts.ApiResponses.Report>>(It.IsAny<GetReportsApiRequest>()))
            .ReturnsAsync((List<Recruit.Contracts.ApiResponses.Report>?)null!);

        var actual = await handler.Handle(new GetReportsQuery(), CancellationToken.None);

        actual.Reports.Should().BeEmpty();
    }
}