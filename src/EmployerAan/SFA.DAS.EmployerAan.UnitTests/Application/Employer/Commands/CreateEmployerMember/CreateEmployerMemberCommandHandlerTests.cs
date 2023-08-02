using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Employer.Commands.CreateEmployerMember;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Employer.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    CreateEmployerMemberCommandHandler sut,
    CreateEmployerMemberCommand command,
    CreateEmployerMemberCommandResult expected,
    CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.PostEmployerMember(command, cancellationToken)).ReturnsAsync(expected);

        var actual = await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.PostEmployerMember(command, cancellationToken));
        actual.Should().Be(expected);
    }
}
