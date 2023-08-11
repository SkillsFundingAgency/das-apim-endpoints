using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Employer.Commands.CreateEmployerMember;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.EmployerControllerTests;
public class CreateEmployerMemberTests
{
    [Test, MoqAutoData]
    public async Task CreateEmployerMember_InvokesCommand(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] EmployersController sut,
    CreateEmployerMemberCommand command,
    CreateEmployerMemberCommandResult expected,
    CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(command, cancellationToken)).ReturnsAsync(expected);

        var response = await sut.CreateEmployerMember(command, cancellationToken);

        mediatorMock.Verify(m => m.Send(command, cancellationToken));
        response.As<OkObjectResult>().Value.Should().Be(expected);
    }
}
