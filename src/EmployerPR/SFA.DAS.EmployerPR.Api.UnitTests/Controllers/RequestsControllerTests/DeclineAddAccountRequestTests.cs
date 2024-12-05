using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class DeclineAddAccountRequestTests
{
    [Test]
    [MoqAutoData]
    public async Task RequestsController_DeclineAddAccountRequest_ReturnsExpectedResponse(DeclinedRequestModel model, Guid requestId)
    {
        RequestsController sut = new RequestsController(Mock.Of<IMediator>());

        var result = await sut.DeclineAddAccountRequest(requestId, model, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }
}