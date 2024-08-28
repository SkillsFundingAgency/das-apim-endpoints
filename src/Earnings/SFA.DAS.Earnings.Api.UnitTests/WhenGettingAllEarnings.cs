using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Earnings.Api.Controllers;
using SFA.DAS.Earnings.Application.Earnings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Earnings.Api.UnitTests;

public class WhenGettingAllEarnings
{
    [Test, MoqAutoData]
    public async Task Then_Gets_All_Earnings_From_Mediator(
        long ukprn,
        GetAllEarningsQueryResult expectedResponse,
        Mock<IMediator> mockMediator,
        Mock<ILogger<EarningsController>> mockLogger)
    {
        // Arrange
        mockMediator
            .Setup(x => x.Send(It.Is<GetAllEarningsQuery>(query => query.Ukprn == ukprn), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var controller = new EarningsController(mockMediator.Object, mockLogger.Object);

        // Act
        var result = await controller.GetAll(ukprn);

        // Assert
        var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
        var actualResponse = okObjectResult.Value.ShouldBeOfType<FM36Learner[]?>();
        actualResponse.Should().BeEquivalentTo(expectedResponse.FM36Learners);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_BadRequest_On_Exception(
        long ukprn,
        Mock<IMediator> mockMediator,
        Mock<ILogger<EarningsController>> mockLogger)
    {
        // Arrange
        var exception = new Exception("Test exception");
        mockMediator
            .Setup(x => x.Send(It.IsAny<GetAllEarningsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var controller = new EarningsController(mockMediator.Object, mockLogger.Object);

        // Act
        var result = await controller.GetAll(ukprn);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}