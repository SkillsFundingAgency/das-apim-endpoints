using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions.GetActivePledgeIdsForAccount;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests;

[TestFixture]
public class GetActivePledgeIdsForAccountTests
{
    private readonly Fixture _fixture = new();

    [Test]
    public async Task Action_Calls_Handler()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        mediator.Setup(x => x.Send(It.IsAny<GetActivePledgeIdsForAccountQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Create<GetActivePledgeIdsForAccountQueryResult>());

        var controller = new FunctionsController(mediator.Object, Mock.Of<ILogger<FunctionsController>>());

        // Act
        await controller.GetActivePledgeIdsForAccount(22, 2, 200);

        // Assert
        mediator.Verify(x => x.Send(
            It.Is<GetActivePledgeIdsForAccountQuery>(q => q.AccountId == 22 && q.Page == 2 && q.PageSize == 200),
            It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Action_Returns_Ok_With_Response()
    {
        // Arrange
        var expected = new GetActivePledgeIdsForAccountQueryResult
        {
            PledgeIds = new[] { 1, 2 },
            Page = 1,
            TotalPages = 3,
            TotalPledges = 2
        };

        var mediator = new Mock<IMediator>();
        mediator.Setup(x => x.Send(It.IsAny<GetActivePledgeIdsForAccountQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new FunctionsController(mediator.Object, Mock.Of<ILogger<FunctionsController>>());

        // Act
        var result = await controller.GetActivePledgeIdsForAccount(22);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var response = ((OkObjectResult)result).Value as GetActivePledgeIdsForAccountResponse;
        response.Should().NotBeNull();
        response!.PledgeIds.Should().BeEquivalentTo(new[] { 1, 2 });
        response.TotalPages.Should().Be(3);
    }
}
