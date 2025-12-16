using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Api.Models.Challenge;
using SFA.DAS.ToolsSupport.Application.Commands.ChallengeEntry;
using SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;
public class ChallengeControllerTests
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Challenge_From_Mediator(
       long accountId,
       GetChallengeQueryResult getChallengeResult,
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] ChallengeController controller)
    {
        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetChallengeQuery>(x =>
                   x.AccountId == accountId),
                   It.IsAny<CancellationToken>())).ReturnsAsync(getChallengeResult);

        var controllerResult = await controller.GetChallenge(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetChallengeResponse;

        model.Should().NotBeNull();
        model.Characters.Should().NotBeNull();
        model.Characters.Should().BeEquivalentTo(getChallengeResult.Characters);
        model.Account.Should().NotBeNull();
        model.Account.Should().BeEquivalentTo(getChallengeResult.Account);
        model.StatusCode.Should().Be(getChallengeResult.StatusCode);
    }

    [Test, MoqAutoData]
    public async Task ChallengeEntry_ShouldReturnOk_WhenRequestIsValid(
        long accountId,
        ChallengeEntryRequest command,
        ChallengeEntryCommandResult result,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ChallengeController controller
        )
    {
        // Arrange
        mockMediator.Setup(m => m.Send(It.Is<ChallengeEntryCommand>(cmd =>
            cmd.AccountId == accountId &&
            cmd.Id == command.Id &&
            cmd.Challenge1 == command.Challenge1 &&
            cmd.Challenge2 == command.Challenge2 &&
            cmd.Balance == command.Balance &&
            cmd.FirstCharacterPosition == command.FirstCharacterPosition &&
            cmd.SecondCharacterPosition == command.SecondCharacterPosition), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result)
            .Verifiable();

        // Act
        var controllerResult = await controller.ChallengeEntry(accountId, command) as ObjectResult;
        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as ChallengeEntryResponse;

        // Assert
        mockMediator.Verify();
        model.Should().NotBeNull();

        model.Should().NotBeNull();
        model.Characters.Should().NotBeNull();
        model.Characters.Should().BeEquivalentTo(result.Characters);
        model.Id.Should().Be(result.Id);
        model.IsValid.Should().Be(result.IsValid);
    }
}
