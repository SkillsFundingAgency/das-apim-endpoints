using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Admins.Commands.Create;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Admins.Command.CreateAdminMember;

public class CreateAdminCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        CreateAdminMemberCommandHandler sut,
        CreateAdminMemberCommand command,
        CreateAdminMemberCommandResult expected,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.CreateAdminMember(command, cancellationToken)).ReturnsAsync(expected);

        var actual = await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.CreateAdminMember(command, cancellationToken));
        actual.Should().Be(expected);
    }
}

