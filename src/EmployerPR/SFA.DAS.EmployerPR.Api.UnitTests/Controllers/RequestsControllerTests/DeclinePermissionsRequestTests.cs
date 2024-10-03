using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class DeclinePermissionsRequestTests
{
    private Mock<IMediator> mediatorMock = new();

    [Test]
    [MoqAutoData]
    public async Task RequestsController_DeclinePermissionsRequest_ReturnsExpectedResponse(DeclinedRequestModel model)
    {
        var requestId = Guid.NewGuid();
        Mock<IProviderRelationshipsApiRestClient> mockApiClient = new();

        mockApiClient.Setup(a =>
            a.DeclineRequest(requestId, model, CancellationToken.None))
        .ReturnsAsync(Unit.Value);

        RequestsController sut = new RequestsController(mockApiClient.Object, mediatorMock.Object);

        var result = await sut.DeclinePermissionsRequest(requestId, model, CancellationToken.None);

        result.Should().BeOfType<OkResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
