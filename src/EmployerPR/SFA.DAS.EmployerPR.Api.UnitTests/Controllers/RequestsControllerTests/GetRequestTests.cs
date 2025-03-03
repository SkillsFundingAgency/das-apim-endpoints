using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.InnerApi.Responses;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class GetRequestTests
{
    [Test]
    [AutoData]
    public async Task RequestsController_GetRequest_ReturnsExpectedResponse(GetRequestResponse expected, Guid requestId, CancellationToken cancellationToken)
    {
        Mock<IMediator> _mediator = new();
        _mediator.Setup(m => m.Send(It.Is<GetRequestQuery>(q => q.RequestId == requestId), cancellationToken)).ReturnsAsync(expected);

        RequestsController sut = new RequestsController(_mediator.Object);

        var actual = await sut.GetRequest(requestId, cancellationToken);

        actual.As<OkObjectResult>().Should().NotBeNull();
        actual.As<OkObjectResult>().Value.Should().Be(expected);
    }

    [Test]
    [AutoData]
    public async Task RequestsController_GetRequest_Returns_NotFound(Guid requestId, CancellationToken cancellationToken)
    {
        Mock<IMediator> _mediator = new();
        _mediator.Setup(m => m.Send(It.Is<GetRequestQuery>(q => q.RequestId == requestId), cancellationToken)).ReturnsAsync(() => null);

        RequestsController sut = new RequestsController(_mediator.Object);

        var result = await sut.GetRequest(requestId, cancellationToken);

        result.As<NotFoundResult>().Should().NotBeNull();
    }
}
