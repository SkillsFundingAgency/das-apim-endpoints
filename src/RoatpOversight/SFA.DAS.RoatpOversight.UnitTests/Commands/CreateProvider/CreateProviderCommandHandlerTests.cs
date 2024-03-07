using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;
using SFA.DAS.RoatpOversight.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.RoatpOversight.UnitTests.Commands.CreateProvider;
public class CreateProviderCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handler_InvokesApi(
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Created, Version = new Version() });

        await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handler_UnexpectedApiResponse_ThrowsInvalidOperation(
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Version = new Version() });


        Func<Task> action = () => sut.Handle(command, cancellationToken);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
