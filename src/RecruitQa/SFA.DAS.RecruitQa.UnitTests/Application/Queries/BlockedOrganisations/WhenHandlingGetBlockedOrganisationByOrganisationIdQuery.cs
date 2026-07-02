using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationByOrganisationId;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.BlockedOrganisations;

[TestFixture]
internal class WhenHandlingGetBlockedOrganisationByOrganisationIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetBlockedOrganisationByOrganisationIdQuery query,
        GetBlockedOrganisationResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetBlockedOrganisationByOrganisationIdQueryHandler handler)
    {
        //Arrange
        var expectedGetUrl = new GetBlockedOrganisationByOrganisationIdRequest(query.OrganisationId);
        recruitApiClient
            .Setup(x => x.Get<GetBlockedOrganisationResponse>(
                It.Is<GetBlockedOrganisationByOrganisationIdRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual!.BlockedOrganisation.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task And_Api_Returns_Null_Then_Returns_Null(
        GetBlockedOrganisationByOrganisationIdQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] GetBlockedOrganisationByOrganisationIdQueryHandler handler)
    {
        //Arrange
        recruitApiClient
            .Setup(x => x.Get<GetBlockedOrganisationResponse>(
                It.IsAny<GetBlockedOrganisationByOrganisationIdRequest>()))
            .ReturnsAsync((GetBlockedOrganisationResponse)null);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeNull();
    }
}
