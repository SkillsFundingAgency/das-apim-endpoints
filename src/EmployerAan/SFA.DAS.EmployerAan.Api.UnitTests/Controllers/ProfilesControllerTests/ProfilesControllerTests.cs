using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.ProfilesControllerTests;

public class ProfilesControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task And_MediatorCommandSuccessful_Then_ReturnOk(GetProfilesByUserTypeQueryResult response,
        [Frozen] Mock<IMediator> mockMediator)
    {
        mockMediator.Setup(m => m.Send(It.IsAny<GetProfilesByUserTypeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new ProfilesController(mockMediator.Object);

        var userType = "employer";
        var result = await controller.GetProfilesByUserType(userType) as OkObjectResult;

        result.Should().NotBeNull();

        var model = result?.Value;

        model.Should().BeEquivalentTo(response);
    }
}
