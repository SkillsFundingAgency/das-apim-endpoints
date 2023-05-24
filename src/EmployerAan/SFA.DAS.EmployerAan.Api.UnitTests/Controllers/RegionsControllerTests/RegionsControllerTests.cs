using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.RegionsControllerTests;

public class RegionsControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task And_MediatorCommandSuccessful_Then_ReturnOk(
    GetRegionsQueryResult response,
    [Frozen] Mock<IMediator> mockMediator,
    CancellationToken cancellationToken)
    {
        mockMediator.Setup(m => m.Send(It.IsAny<GetRegionsQuery>(), cancellationToken)).ReturnsAsync(response);
        var controller = new RegionsController(mockMediator.Object);

        var result = await controller.GetRegions(cancellationToken);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}