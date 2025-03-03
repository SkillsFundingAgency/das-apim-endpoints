using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineCreateAccountRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.RequestsControllerTests;

public sealed class DeclineCreateAccountRequestTests
{
    [Test]
    [MoqAutoData]
    public async Task RequestsController_DeclineCreateAccountRequest_ReturnsExpectedResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RequestsController sut,
        DeclinedRequestModel model, Guid requestId)
    {
        var result = await sut.DeclineCreateAccountRequest(requestId, model, CancellationToken.None);

        mediatorMock.Verify(
            m => m.Send(It.Is<DeclineCreateAccountRequestCommand>(x => x.ActionedBy == model.ActionedBy && x.RequestId == requestId),
                It.IsAny<CancellationToken>()));
        result.Should().BeOfType<OkResult>();
    }
}