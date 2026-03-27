using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Commands.UpsertBlockedOrganisation;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationByOrganisationId;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Queries.GetBlockedOrganisationsByOrganisationType;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.BlockedOrganisation;

public class WhenGettingBlockedOrganisation
{
    [Test, MoqAutoData]
    public async Task Then_Gets_BlockedOrganisation_From_Mediator(
        string organisationId,
        GetBlockedOrganisationByOrganisationIdQueryResult mediatorResponse,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BlockedOrganisationController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetBlockedOrganisationByOrganisationIdQuery>(c => c.OrganisationId == organisationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var result = await controller.GetBlockedOrganisation(organisationId, It.IsAny<CancellationToken>());
        var payload = (result as Ok<BlockedOrganisationRequestDto>)?.Value;

        // assert
        mockMediator.Verify(mediator => mediator.Send(
            It.Is<GetBlockedOrganisationByOrganisationIdQuery>(c => c.OrganisationId == organisationId),
            It.IsAny<CancellationToken>()), Times.Once);
        payload.Should().NotBeNull();
        payload.Should().BeEquivalentTo(mediatorResponse.BlockedOrganisation);
    }

    [Test, MoqAutoData]
    public async Task And_Result_Is_Null_Then_Returns_NotFound(
        string organisationId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BlockedOrganisationController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetBlockedOrganisationByOrganisationIdQuery>(c => c.OrganisationId == organisationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetBlockedOrganisationByOrganisationIdQueryResult)null!);

        var result = await controller.GetBlockedOrganisation(organisationId, It.IsAny<CancellationToken>());

        result.Should().BeOfType<NotFound>();
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Problem(
        string organisationId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BlockedOrganisationController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetBlockedOrganisationByOrganisationIdQuery>(),
                It.IsAny<CancellationToken>()))
           .Throws<InvalidOperationException>();

        var actual = await controller.GetBlockedOrganisation(organisationId, It.IsAny<CancellationToken>());

        actual.Should().NotBeNull();
        actual.Should().BeOfType<ProblemHttpResult>();
    }
}
