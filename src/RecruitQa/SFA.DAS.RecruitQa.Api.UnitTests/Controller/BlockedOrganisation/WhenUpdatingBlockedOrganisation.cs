using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.BlockedOrganisations.Commands.UpsertBlockedOrganisation;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.BlockedOrganisation;

public class WhenUpdatingBlockedOrganisation
{
    [Test, MoqAutoData]
    public async Task Then_Updates_BlockedOrganisation(
        BlockedOrganisationRequestDto request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BlockedOrganisationController controller)
    {
        var actual = await controller.UpdateBlockedOrganisation(request, It.IsAny<CancellationToken>());

        mockMediator.Verify(mediator => mediator.Send(
            It.Is<UpsertBlockedOrganisationCommand>(c =>
                c.Id == request.Id &&
                c.OrganisationId == request.OrganisationId &&
                c.OrganisationType == request.OrganisationType &&
                c.BlockedStatus == request.BlockedStatus &&
                c.Reason == request.Reason &&
                c.UpdatedByUserId == request.UpdatedByUserId &&
                c.UpdatedByUserEmail == request.UpdatedByUserEmail
            ),
            It.IsAny<CancellationToken>()), Times.Once);
        actual.Should().BeOfType<Created>();
    }
    
    
    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Problem(
        BlockedOrganisationRequestDto request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] BlockedOrganisationController controller,
        CancellationToken token)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<UpsertBlockedOrganisationCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var actual = await controller.UpdateBlockedOrganisation(request, It.IsAny<CancellationToken>());

        actual.Should().NotBeNull();
        actual.Should().BeOfType<ProblemHttpResult>();
    }
}