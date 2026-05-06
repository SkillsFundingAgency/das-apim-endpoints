using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.OrganisationsControllerTests;
public class OrganisationsControllerGetOrganisationTests
{
    [Test, MoqAutoData]
    public async Task GetOrganisationByUkprn_ReturnSuccessfulResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationsController sut,
        GetOrganisationQuery request,
        GetOrganisationQueryResult expectedResponse)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetOrganisationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        var result = await sut.GetOrganisation(request.ukprn, It.IsAny<CancellationToken>());
        var response = (OkObjectResult)result;

        mediatorMock.Verify(m => m.Send(It.Is<GetOrganisationQuery>(r => r.ukprn == request.ukprn), It.IsAny<CancellationToken>()), Times.Once());
        response.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Test, MoqAutoData]
    public async Task GetOrganisationByUkprn_ReturnNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationsController sut,
        GetOrganisationQuery request)
    {
        GetOrganisationQueryResult? expectedResponse = null;
        mediatorMock.Setup(m => m.Send(It.IsAny<GetOrganisationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        var result = await sut.GetOrganisation(request.ukprn, It.IsAny<CancellationToken>());

        mediatorMock.Verify(m => m.Send(It.Is<GetOrganisationQuery>(r => r.ukprn == request.ukprn), It.IsAny<CancellationToken>()), Times.Once());
        result.Should().BeOfType<NotFoundResult>();
    }
}