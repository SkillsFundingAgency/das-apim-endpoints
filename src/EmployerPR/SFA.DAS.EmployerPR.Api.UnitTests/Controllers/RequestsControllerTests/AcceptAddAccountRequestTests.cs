using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptAddAccountRequest;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public class AcceptAddAccountRequestTests
{
    [Test]
    [AutoData]
    public async Task RequestsController_AcceptAddAccountRequest_ReturnsExpectedResponse(Guid requestId, AcceptAddAccountRequestModel model)
    {
        RequestsController sut = new RequestsController(Mock.Of<IMediator>());

        var result = await sut.AcceptAddAccountRequest(requestId, model, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }
}
