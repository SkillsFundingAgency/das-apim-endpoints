using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptAddAccountRequest;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class AcceptAddAccountRequestTests
{
    private Mock<IMediator> mediatorMock = new();

    [Test]
    [MoqAutoData]
    public async Task RequestsController_AcceptAddAccountRequest_ReturnsExpectedResponse(AcceptAddAccountRequestModel model)
    {
        var requestId = Guid.NewGuid();
        Mock<IProviderRelationshipsApiRestClient> mockApiClient = new();

        mockApiClient.Setup(a =>
            a.AcceptAddAccountRequest(requestId, model, CancellationToken.None))
        .ReturnsAsync(Unit.Value);

        RequestsController sut = new RequestsController(mockApiClient.Object, mediatorMock.Object);

        var result = await sut.AcceptAddAccountRequest(requestId, model, CancellationToken.None);

        result.Should().BeOfType<OkResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
