using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetQaDashboard;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Dashboard;
[TestFixture]
internal class WhenHandlingGetQaDashboardQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetQaDashboardQuery query,
        GetQaDashboardApiResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetQaDashboardQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetQaDashboardApiRequest();
        recruitApiClient
            .Setup(x => x.Get<GetQaDashboardApiResponse>(
                It.Is<GetQaDashboardApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.Get<GetQaDashboardApiResponse>(It.IsAny<GetQaDashboardApiRequest>()), Times.Once);
    }
}