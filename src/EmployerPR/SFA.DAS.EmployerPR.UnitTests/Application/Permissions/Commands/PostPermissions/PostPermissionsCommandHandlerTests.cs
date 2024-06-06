using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Commands.PostPermissions;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_Returns_PostPermissionsCommandResult(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommand command,
        PostPermissionsCommandResult result
    )
    {
        providerRelationshipsApiRestClient.Setup(x =>
            x.PostPermissions(
                command,
                CancellationToken.None
            )
        ).ReturnsAsync(result);

        var actual = await sut.Handle(command, CancellationToken.None);
        actual.Should().Be(result);
    }
}