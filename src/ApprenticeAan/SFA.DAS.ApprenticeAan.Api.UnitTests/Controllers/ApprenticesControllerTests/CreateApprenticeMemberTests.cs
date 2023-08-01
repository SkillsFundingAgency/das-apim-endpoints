using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.ApprenticesControllerTests;
public class CreateApprenticeMemberTests
{
    [Test, MoqAutoData]
    public async Task CreateApprenticeMember_InvokesCommand(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        CreateApprenticeMemberCommand command,
        CreateApprenticeMemberCommandResult expected,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).ReturnsAsync(expected);

        var response = await sut.CreateApprenticeMember(command, cancellationToken);

        mediatorMock.Verify(m => m.Send(command, cancellationToken));
        response.As<OkObjectResult>().Value.Should().Be(expected);
    }
}
