using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class DeclinePermissionsRequestTests
{
    [Test]
    [MoqAutoData]
    public async Task RequestsController_DeclinePermissionsRequest_ReturnsExpectedResponse(DeclinedRequestModel model, Guid requestId)
    {
        RequestsController sut = new RequestsController(Mock.Of<IMediator>());

        var result = await sut.DeclinePermissionsRequest(requestId, model, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }
}
