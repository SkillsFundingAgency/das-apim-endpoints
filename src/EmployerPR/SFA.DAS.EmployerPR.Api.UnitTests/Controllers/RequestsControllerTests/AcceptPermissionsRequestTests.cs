using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptPermissionsRequest;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public class AcceptPermissionsRequestTests
{
    [Test]
    [AutoData]
    public async Task RequestsController_AcceptPermissionsRequest_ReturnsExpectedResponse(AcceptPermissionsRequestModel model, Guid requestId)
    {
        RequestsController sut = new RequestsController(Mock.Of<IMediator>());

        var result = await sut.AcceptPermissionsRequest(requestId, model, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }
}
