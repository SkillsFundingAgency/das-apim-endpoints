using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class GetRequestTests
{
    [Test]
    [MoqAutoData]
    public async Task RequestsController_GetRequest_ReturnsExpectedResponse(GetRequestResponse response)
    {
        Mock<IProviderRelationshipsApiRestClient> mockApiClient = new();
        Mock<IMediator> mediatorMock = new();
        mockApiClient.Setup(a =>
            a.GetRequest(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(response);

        RequestsController sut = new RequestsController(mockApiClient.Object, mediatorMock.Object);
       
        var result = await sut.GetRequest(It.IsAny<Guid>(), CancellationToken.None);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
    [Test]
    [MoqAutoData]
    public async Task RequestsController_GetRequest_Returns_NotFound()
    {
        Mock<IProviderRelationshipsApiRestClient> mockApiClient = new();
        Mock<IMediator> mediatorMock = new();
        mockApiClient.Setup(a =>
            a.GetRequest(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((GetRequestResponse?)null);

        RequestsController sut = new RequestsController(mockApiClient.Object, mediatorMock.Object);

        var result = await sut.GetRequest(It.IsAny<Guid>(), CancellationToken.None);

        result.As<NotFoundResult>().Should().NotBeNull();
        result.As<NotFoundResult>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
