using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.CleanupPledgeForNonLevy;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests;

[TestFixture]
public class CleanupPledgeForNonLevyTests
{
    [Test]
    public async Task Action_Calls_Handler()
    {
        // Arrange
        var fixture = new Fixture();
        var request = fixture.Create<CleanupPledgeForNonLevyRequest>();

        var mediator = new Mock<IMediator>();

        var controller = new FunctionsController(mediator.Object, Mock.Of<ILogger<FunctionsController>>());

        // Act
        await controller.CleanupPledgeForNonLevy(request);

        // Assert
        mediator.Verify(x => x.Send(
            It.Is<CleanupPledgeForNonLevyCommand>(c => c.AccountId == request.AccountId && c.PledgeId == request.PledgeId),
            It.IsAny<CancellationToken>()));
    }
}
