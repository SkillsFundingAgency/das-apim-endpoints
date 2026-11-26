using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Standards.Queries.GetStandards;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Standards;

public class StandardsControllerTests
{
    [Test]
    [AutoData]
    public async Task GetStandards_CallsMediatorAndReturnsOkObjectResult(GetStandardsQueryResult expectedResult)
    {
        Mock<IMediator> mockMediator = new Mock<IMediator>();
        mockMediator
            .Setup(mediator => mediator.Send(It.IsAny<GetStandardsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var sut = new StandardsController(mockMediator.Object);

        var result = await sut.GetStandards();

        mockMediator.Verify(x => x.Send(It.IsAny<GetStandardsQuery>(), It.IsAny<CancellationToken>()), Times.Once);

        result.As<OkObjectResult>().Value.Should().Be(expectedResult);
    }
}
