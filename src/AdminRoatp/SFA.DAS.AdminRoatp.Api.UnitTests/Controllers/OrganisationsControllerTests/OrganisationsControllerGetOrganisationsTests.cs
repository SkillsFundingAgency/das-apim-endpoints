using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.OrganisationsControllerTests;
public class OrganisationsControllerGetOrganisationsTests
{
    [Test, MoqAutoData]
    public async Task GetOrganisations_ReturnSuccessfulResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationsController sut,
        GetOrganisationsResponse expectedResponse)
    {
        // Arrange
        mediatorMock.Setup(m => m.Send(It.IsAny<GetOrganisationsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        // Act
        var result = await sut.GetOrganisations(It.IsAny<CancellationToken>());
        var response = (OkObjectResult)result;

        // Assert
        mediatorMock.Verify(m => m.Send(It.IsAny<GetOrganisationsQuery>(), It.IsAny<CancellationToken>()), Times.Once());
        response.Value.Should().BeEquivalentTo(expectedResponse);
    }
}