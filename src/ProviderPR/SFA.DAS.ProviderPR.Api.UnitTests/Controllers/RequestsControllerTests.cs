using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.ProviderPR.Api.Controllers;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AccountInvitation;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;
using SFA.DAS.ProviderPR.Application.Requests.Commands.CreatePermissions;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.UnitTests.Controllers;

public class RequestsControllerTests
{
    [Test]
    [AutoData]
    public async Task RequestsController_AddAccount_ReturnsOkResponse(
        AddAccountRequestCommand command,
        AddAccountRequestCommandResult commandResult,
        CancellationToken cancellationToken
    )
    {
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(c => c.Send(It.IsAny<AddAccountRequestCommand>(), cancellationToken)).ReturnsAsync(commandResult);

        RequestsController sut = new(mediatorMock.Object, new Mock<IProviderRelationshipsApiRestClient>().Object);

        var result = await sut.AddAccount(command, cancellationToken);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.As<AddAccountRequestCommandResult>().RequestId.Should().Be(commandResult.RequestId);
    }

    [Test]
    [AutoData]
    public async Task RequestsController_GetRequestFromRequestId_ReturnsOkResponse(
        Guid requestId,
        GetRequestResponse response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.GetRequest(requestId, CancellationToken.None)).ReturnsAsync(response);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.GetRequest(requestId, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    [AutoData]
    public async Task RequestsController_GetRequestFromRequestId_ReturnsNotFoundResult(
        Guid requestId,
        GetRequestResponse response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.GetRequest(requestId, CancellationToken.None)).ReturnsAsync((GetRequestResponse?)null);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.GetRequest(requestId, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, AutoData]
    public async Task RequestsController_GetRequestFromUkprnPaye_ReturnsOkResponse(
        int ukprn,
        string paye,
        GetRequestResponse response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.GetRequest(ukprn, paye, null, CancellationToken.None)).ReturnsAsync(response);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.GetRequest(ukprn, paye, null, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Test, AutoData]
    public async Task RequestsController_GetRequestFromUkprnPaye_ReturnsNotFoundResult(
        int ukprn,
        string paye,
        GetRequestResponse response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.GetRequest(ukprn, paye, null, CancellationToken.None)).ReturnsAsync((GetRequestResponse?)null);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.GetRequest(ukprn, paye, null, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, AutoData]
    public async Task RequestsController_GetRequestFromUkprnEmail_ReturnsOkResponse(
        int ukprn,
        string email,
        GetRequestResponse response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.GetRequest(ukprn, null, email, CancellationToken.None)).ReturnsAsync(response);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.GetRequest(ukprn, null, email, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Test, AutoData]
    public async Task RequestsController_GetRequestFromUkprnEmail_ReturnsNotFoundResult(
        int ukprn,
        string email,
        GetRequestResponse response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.GetRequest(ukprn, null, email, CancellationToken.None)).ReturnsAsync((GetRequestResponse?)null);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.GetRequest(ukprn, null, email, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    [AutoData]
    public async Task RequestsController_CreatePermissions_ReturnsOkResponse(
        CreatePermissionRequestCommand command,
        CreatePermissionRequestCommandResult response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.CreatePermissionsRequest(It.IsAny<CreatePermissionRequestCommand>(), CancellationToken.None)).ReturnsAsync(response);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.CreatePermissions(command, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    [AutoData]
    public void RequestsController_CreatePermissions_ReturnsBadRequest(
        CreatePermissionRequestCommand command,
        CreatePermissionRequestCommandResult response
    )
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(a =>
            a.Send(
                    It.IsAny<CreatePermissionRequestCommand>(),
                    It.IsAny<CancellationToken>()
                )
            ).ThrowsAsync(
                new ApiException(
                    new HttpRequestMessage(),
                    new HttpResponseMessage(HttpStatusCode.BadRequest),
                    "RestEase.ApiException: POST failed because response status code does not indicate success: 400(Bad Request)."
                )
        );

        RequestsController sut = new(mediator.Object, new Mock<IProviderRelationshipsApiRestClient>().Object);

        Assert.ThrowsAsync<ApiException>(async () => await sut.CreatePermissions(command, CancellationToken.None));
    }

    [Test]
    [AutoData]
    public async Task RequestsController_CreateAccount_ReturnsOkResponse(
        CreateAccountInvitationRequestCommand command,
        CreateAccountInvitationRequestCommandResult response
    )
    {
        Mock<IProviderRelationshipsApiRestClient> client = new();
        client.Setup(c => c.CreateAccountInvitationRequest(It.IsAny<CreateAccountInvitationRequestCommand>(), CancellationToken.None)).ReturnsAsync(response);

        RequestsController sut = new(new Mock<IMediator>().Object, client.Object);

        var result = await sut.CreateAccount(command, CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    [AutoData]
    public void RequestsController_CreateAccount_ReturnsBadRequest(
        CreateAccountInvitationRequestCommand command,
        CreatePermissionRequestCommandResult response
    )
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(a =>
            a.Send(
                    It.IsAny<CreateAccountInvitationRequestCommand>(),
                    It.IsAny<CancellationToken>()
                )
            ).ThrowsAsync(
                new ApiException(
                    new HttpRequestMessage(),
                    new HttpResponseMessage(HttpStatusCode.BadRequest),
                    "RestEase.ApiException: POST failed because response status code does not indicate success: 400(Bad Request)."
                )
        );

        RequestsController sut = new(mediator.Object, new Mock<IProviderRelationshipsApiRestClient>().Object);

        Assert.ThrowsAsync<ApiException>(async () => await sut.CreateAccount(command, CancellationToken.None));
    }
}
