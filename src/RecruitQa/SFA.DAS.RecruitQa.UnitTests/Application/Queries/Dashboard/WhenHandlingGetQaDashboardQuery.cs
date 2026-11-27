using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetQaDashboard;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.Dashboard;

[TestFixture]
internal class WhenHandlingGetQaDashboardQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetQaDashboardQuery query,
        QaDashboard apiResponse,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        [Greedy] GetQaDashboardQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetVacancyreviewsQaDashboardApiRequest();
        recruitApiClient
            .Setup(x => x.Get<QaDashboard>(
                It.Is<GetVacancyreviewsQaDashboardApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.Get<QaDashboard>(It.IsAny<GetVacancyreviewsQaDashboardApiRequest>()), Times.Once);
    }
}