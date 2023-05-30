using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Apprentices.Commands.CreateApprenticeMember;

public class CreateApprenticeMemberCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        CreateApprenticeMemberCommandHandler sut,
        CreateApprenticeMemberCommand command,
        CreateApprenticeMemberCommandResult expected,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.PostApprenticeMember(command, cancellationToken)).ReturnsAsync(expected);

        var actual = await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.PostApprenticeMember(command, cancellationToken));
        actual.Should().Be(expected);
    }
}
