using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Api.Models;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptCreateAccountRequest;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public class AcceptCreateAccountRequestTests
{
    [Test]
    [AutoData]
    public async Task AcceptCreateAccountRequest_InvokeMediatorAndReturnsOkResult(Guid requestId, AcceptCreateAccountRequestModel model, CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        RequestsController sut = new RequestsController(mediatorMock.Object);

        var result = await sut.AcceptCreateAccountRequest(requestId, model, cancellationToken);

        mediatorMock.Verify(m => m.Send(
            It.Is<AcceptCreateAccountRequestCommand>(c => c.RequestId == requestId && c.FirstName == model.FirstName && c.LastName == model.LastName && c.Email == model.Email && c.GovIdentifier == model.GovIdentifier)
            , cancellationToken), Times.Once);

        result.Should().BeOfType<OkResult>();
    }
}
