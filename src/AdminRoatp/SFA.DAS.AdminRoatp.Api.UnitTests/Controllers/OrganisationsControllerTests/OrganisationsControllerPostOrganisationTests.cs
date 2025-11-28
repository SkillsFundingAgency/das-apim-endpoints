using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.OrganisationsControllerTests;
public class OrganisationsControllerPostOrganisationTests
{
    [Test, MoqInlineAutoData]
    public async Task PostOrganisation_StatusCreated(int ukprn, PostOrganisationRequest request)
    {
        var command = (PostOrganisationCommand)request;
        command.Ukprn = ukprn;
        var mediatorMock = new Mock<IMediator>();
        var sut = new OrganisationsController(mediatorMock.Object, Mock.Of<ILogger<OrganisationsController>>());

        mediatorMock.Setup(m => m.Send(It.Is<PostOrganisationCommand>(c => c.Ukprn == ukprn && c.LegalName == request.LegalName), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.Created);
        var result = await sut.PostOrganisation(ukprn, request, CancellationToken.None);
        mediatorMock.Verify(m => m.Send(It.Is<PostOrganisationCommand>(c => c.Ukprn == ukprn && c.LegalName == request.LegalName), It.IsAny<CancellationToken>()), Times.Once);
        result.As<StatusCodeResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Test, MoqInlineAutoData]
    public async Task PostOrganisation_BadRequest(int ukprn, PostOrganisationRequest request)
    {
        var command = (PostOrganisationCommand)request;
        command.Ukprn = ukprn;
        var mediatorMock = new Mock<IMediator>();
        var sut = new OrganisationsController(mediatorMock.Object, Mock.Of<ILogger<OrganisationsController>>());

        mediatorMock.Setup(m => m.Send(It.Is<PostOrganisationCommand>(c => c.Ukprn == ukprn && c.LegalName == request.LegalName), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.BadRequest);
        var result = await sut.PostOrganisation(ukprn, request, CancellationToken.None);
        mediatorMock.Verify(m => m.Send(It.Is<PostOrganisationCommand>(c => c.Ukprn == ukprn && c.LegalName == request.LegalName), It.IsAny<CancellationToken>()), Times.Once);
        result.As<StatusCodeResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
