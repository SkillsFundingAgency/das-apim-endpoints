using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.LeavingReasonsControllerTests;
public class LeavingReasonsControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task And_ApiClientCallSuccessful_Then_ReturnOk(
        List<LeavingCategory> response,
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetLeavingReasons(cancellationToken)).ReturnsAsync(response);
        var controller = new LeavingReasonsController(apiClient.Object);

        var result = await controller.GetLeavingReasons(cancellationToken);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}