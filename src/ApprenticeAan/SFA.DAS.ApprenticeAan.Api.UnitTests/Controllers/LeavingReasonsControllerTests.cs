using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Model;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers;

public class LeavingReasonsControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task And_ApiClientCommandSuccessful_Then_ReturnOk(
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