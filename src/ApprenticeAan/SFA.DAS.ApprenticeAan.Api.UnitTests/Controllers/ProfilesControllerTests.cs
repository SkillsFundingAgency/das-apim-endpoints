using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers;

public class ProfilesControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task And_MediatorCommandSuccessful_Then_ReturnOk(
        GetProfilesByUserTypeQueryResult response,
        [Frozen] Mock<IMediator> mockMediator,
        CancellationToken cancellationToken)
    {
        mockMediator.Setup(m => m.Send(It.IsAny<GetProfilesByUserTypeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new ProfilesController(mockMediator.Object);
        var userType = "Apprentice";

        var result = await controller.GetProfilesByUserType(userType, cancellationToken);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}