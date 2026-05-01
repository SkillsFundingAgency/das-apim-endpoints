using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationsByOrganisationType;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.BlockedOrganisations;

[TestFixture]
internal class WhenHandlingGetBlockedOrganisationsByOrganisationTypeQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetBlockedOrganisationsByOrganisationTypeQuery query,
        List<GetBlockedOrganisationResponse> apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetBlockedOrganisationsByOrganisationTypeQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetBlockedOrganisationsByOrganisationTypeRequest(query.OrganisationType);
        recruitApiClient
            .Setup(x => x.GetAll<GetBlockedOrganisationResponse>(
                It.Is<GetBlockedOrganisationsByOrganisationTypeRequest>(r => r.GetAllUrl == expectedGetUrl.GetAllUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.BlockedOrganisations.Should().BeEquivalentTo(apiResponse);
        recruitApiClient.Verify(x => x.GetAll<GetBlockedOrganisationResponse>(It.IsAny<GetBlockedOrganisationsByOrganisationTypeRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Api_Returns_Null_Then_Returns_Empty_List(
        GetBlockedOrganisationsByOrganisationTypeQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetBlockedOrganisationsByOrganisationTypeQueryHandler handler)
    {
        //Arrange
        recruitApiClient
            .Setup(x => x.GetAll<GetBlockedOrganisationResponse>(
                It.IsAny<GetBlockedOrganisationsByOrganisationTypeRequest>()))
            .ReturnsAsync((IEnumerable<GetBlockedOrganisationResponse>)null);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.BlockedOrganisations.Should().BeEmpty();
    }
}
