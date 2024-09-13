using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineRequest;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class DeclineRequestTests
{
    private Mock<IMediator> mediatorMock = new();

    [Test]
    [MoqAutoData]
    public async Task RequestsController_DeclineRequest_ReturnsExpectedResponse(DeclinedRequestModel model)
    {
        var requestId = Guid.NewGuid();
        Mock<IProviderRelationshipsApiRestClient> mockApiClient = new();

        mockApiClient.Setup(a =>
            a.DeclineRequest(requestId, model, CancellationToken.None))
        .ReturnsAsync(Unit.Value);

        RequestsController sut = new RequestsController(mockApiClient.Object, mediatorMock.Object);

        var result = await sut.DeclineRequest(requestId, model, CancellationToken.None);

        result.Should().BeOfType<OkResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
