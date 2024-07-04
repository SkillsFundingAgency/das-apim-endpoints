using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.Commands.PostPermissions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.PermissionsControllerTests;

public class PostPermissionsTests
{
    [Test]
    [MoqAutoData]
    public async Task PostPermissions_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        PostPermissionsCommand command,
        CancellationToken cancellationToken
    )
    {
        await sut.PostPermissions(command, cancellationToken);
        mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task PostPermissions_HandlerReturnsData_ReturnsNoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        PostPermissionsCommand command,
        PostPermissionsCommandResult postPermissionsCommandResult,
        CancellationToken cancellationToken
    )
    {
        mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()));

        var result = await sut.PostPermissions(command, cancellationToken);

        result.As<NoContentResult>().Should().NotBeNull();
    }
}
