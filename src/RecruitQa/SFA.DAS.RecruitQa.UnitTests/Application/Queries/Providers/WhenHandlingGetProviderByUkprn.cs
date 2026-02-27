using SFA.DAS.RecruitQa.Application.Provider.GetProvider;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCourses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.Providers;

[TestFixture]
internal class WhenHandlingGetProviderByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetProviderQuery query,
        GetProvidersListItem apiResponse,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCourseManagementApiClient,
        [Greedy] GetProviderQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetProviderRequest(query.Ukprn);
        roatpCourseManagementApiClient
            .Setup(x => x.Get<GetProvidersListItem>(
                It.Is<GetProviderRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Provider.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        roatpCourseManagementApiClient.Verify(x => x.Get<GetProvidersListItem>(It.IsAny<GetProviderRequest>()), Times.Once);
    }
}