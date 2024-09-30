using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptPermissionsRequest;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class AcceptPermissionsRequestTests
{
    private Mock<IMediator> mediatorMock = new();

    [Test]
    [MoqAutoData]
    public async Task RequestsController_AcceptPermissionsRequest_ReturnsExpectedResponse(AcceptPermissionsRequestModel model)
    {
        var requestId = Guid.NewGuid();
        Mock<IProviderRelationshipsApiRestClient> mockApiClient = new();

        mockApiClient.Setup(a =>
            a.AcceptPermissionsRequest(requestId, model, CancellationToken.None))
        .ReturnsAsync(Unit.Value);

        RequestsController sut = new RequestsController(mockApiClient.Object, mediatorMock.Object);

        var result = await sut.AcceptPermissionsRequest(requestId, model, CancellationToken.None);

        result.Should().BeOfType<OkResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
