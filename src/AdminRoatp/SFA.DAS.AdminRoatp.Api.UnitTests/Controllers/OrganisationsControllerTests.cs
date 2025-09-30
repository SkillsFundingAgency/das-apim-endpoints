using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers;
public class OrganisationsControllerTests
{
    [Test, MoqAutoData]
    public async Task GetOrganisations(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationController sut,
        GetOrganisationsQuery request,
        GetOrganisationsQueryResponse expectedResponse)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetOrganisationsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        var result = await sut.GetOrganisations(request.SearchTerm, It.IsAny<CancellationToken>());
        var response = (OkObjectResult)result;

        mediatorMock.Verify(m => m.Send(It.Is<GetOrganisationsQuery>(r => r.SearchTerm == request.SearchTerm), It.IsAny<CancellationToken>()), Times.Once());
        response.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Test, MoqAutoData]
    public async Task GetOrganisation(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationController sut,
        GetOrganisationQuery request,
        GetOrganisationQueryResponse expectedResponse)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetOrganisationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        var result = await sut.GetOrganisation(request.ukprn, It.IsAny<CancellationToken>());
        var response = (OkObjectResult)result;

        mediatorMock.Verify(m => m.Send(It.Is<GetOrganisationQuery>(r => r.ukprn == request.ukprn), It.IsAny<CancellationToken>()), Times.Once());
        response.Value.Should().BeEquivalentTo(expectedResponse);
    }
}