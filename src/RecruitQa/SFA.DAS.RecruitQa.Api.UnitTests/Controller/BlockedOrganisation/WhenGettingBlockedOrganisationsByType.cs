using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationsByOrganisationType;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.BlockedOrganisation;

public class WhenGettingBlockedOrganisationsByType
{
    [Test, MoqAutoData]
    public async Task Then_Gets_BlockedOrganisations_By_Type(
        string organisationType,
        GetGetBlockedOrganisationsByOrganisationTypeQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BlockedOrganisationController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetBlockedOrganisationsByOrganisationTypeQuery>(c => c.OrganisationType == organisationType),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var actual = await controller.GetManyByOrganisationType(organisationType, It.IsAny<CancellationToken>());
        var actualResponse = (actual as Ok<List<BlockedOrganisationRequestDto>>)?.Value;

        // assert
        mockMediator.Verify(mediator => mediator.Send(
            It.Is<GetBlockedOrganisationsByOrganisationTypeQuery>(c => c.OrganisationType == organisationType),
            It.IsAny<CancellationToken>()), Times.Once);
        actualResponse.Should().NotBeNull();
        actualResponse.Should().BeEquivalentTo(mediatorResponse.BlockedOrganisations.Select(c => (BlockedOrganisationRequestDto)c).ToList());
    }
    
    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Problem(
        string organisationType,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BlockedOrganisationController controller,
        CancellationToken token)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetBlockedOrganisationsByOrganisationTypeQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var actual = await controller.GetManyByOrganisationType(organisationType, It.IsAny<CancellationToken>());

        actual.Should().NotBeNull();
        actual.Should().BeOfType<ProblemHttpResult>();
    }
}