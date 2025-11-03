using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisationTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.OrganisationTypesTests;
public class OrganisationTypesControllerGetOrganisationTypesTests
{
    [Test, MoqAutoData]
    public async Task Test(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationTypesController sut,
        GetOrganisationTypesResponse response)
    {
        // Arrange
        mediatorMock.Setup(m => m.Send(It.IsAny<GetOrganisationTypesQuery>(), CancellationToken.None)).ReturnsAsync(response);

        // Act
        var result = await sut.GetOrganisationTypes(CancellationToken.None);

        // Assert
        result.As<OkObjectResult>().Value.Should().Be(response);
        mediatorMock.Verify(m => m.Send(It.IsAny<GetOrganisationTypesQuery>(), CancellationToken.None), Times.Once());
    }
}